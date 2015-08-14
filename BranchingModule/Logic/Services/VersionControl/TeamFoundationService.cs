using System;
using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class TeamFoundationService : IVersionControlService
	{
		#region Properties
		public ITextOutputService TextOutput { get; set; }

		private IConvention Convention { get; set; }

		private ISettings Settings { get; set; }

		private ITeamFoundationVersionControlAdapter VersionControlAdapter { get; set; }
		#endregion

		#region Constructors
		public TeamFoundationService(ITeamFoundationVersionControlAdapter versoControlAdapterAdapter, IConvention convention, ISettings settings, ITextOutputService textOutputService)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControlAdapter = versoControlAdapterAdapter;
			this.Convention = convention;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void CreateMapping(BranchInfo branch)
		{
			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			if(!this.VersionControlAdapter.ServerItemExists(strServerPath)) throw new ArgumentException(string.Format("Serverpath {0} for {1} does not exist.", strServerPath, branch));

			this.TextOutput.WriteVerbose(string.Format("Mapping {0} to {1}", strServerPath, strLocalPath));

			this.VersionControlAdapter.CreateMapping(strServerPath, strLocalPath);
			this.VersionControlAdapter.Get(strServerPath);
		}

		public void DeleteMapping(BranchInfo branch)
		{
			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			this.TextOutput.WriteVerbose(string.Format("Unmapping {0}", branch));

			this.VersionControlAdapter.DeleteMapping(strServerPath, strLocalPath);
			this.VersionControlAdapter.Get();
		}

		public void CreateAppConfig(BranchInfo branch)
		{
			string strLocalPath = string.Format(@"{0}\Web\app.config", this.Convention.GetLocalPath(branch));

			this.VersionControlAdapter.DownloadFile(this.Settings.AppConfigServerPath, strLocalPath);
		}

		public DateTime GetCreationTime(BranchInfo branch)
		{
			string strItem = string.Format(@"{0}/{1}.nuspec", this.Convention.GetServerPath(this.Convention.MainBranch(branch.TeamProject)), branch.TeamProject);
			string strVersionSpec = GetVersionSpec(branch);

			return this.VersionControlAdapter.GetCreationTime(strItem, strVersionSpec);
		}

		public void CreateBranch(BranchInfo branch)
		{
			string strSourceBranch = this.Convention.GetServerPath(this.Convention.MainBranch(branch.TeamProject));
			string strTargetBranch = this.Convention.GetServerPath(branch);
			string strVersionByLabel = GetVersionSpec(branch);

			if(this.VersionControlAdapter.ServerItemExists(strTargetBranch))
			{
				this.TextOutput.WriteVerbose(string.Format("Branch {0} already exists. Skipping...", strTargetBranch));
				return;
			}

			this.VersionControlAdapter.CreateBranch(strSourceBranch, strTargetBranch, strVersionByLabel);
		}

		public void DeleteBranch(BranchInfo branch)
		{
			string strBranchBasePath = this.Convention.GetServerBasePath(branch);

			if(!this.VersionControlAdapter.ServerItemExists(strBranchBasePath))
			{
				this.TextOutput.WriteVerbose(string.Format("{0} is already deleted. Skipping...", strBranchBasePath));
				return;
			}

			this.TextOutput.WriteVerbose(string.Format("Destroying {0}", strBranchBasePath));
			this.VersionControlAdapter.DeleteBranch(strBranchBasePath);
		}

		public BranchInfo GetBranchInfoByChangeset(string strChangeset)
		{
			var affectedBranches = (from item in this.VersionControlAdapter.GetServerItemsByChangeset(strChangeset)
			                        select this.Convention.GetBranchInfoByServerPath(item)).Distinct();

			return affectedBranches.Single();
		}

		public void MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, ISet<BranchInfo> targetBranches)
		{
			MergeChangeset(strChangesetToMerge, sourceBranch, targetBranches, true);
		}

		public void MergeChangesetWithoutCheckIn(string strChangesetToMerge, BranchInfo sourceBranch, ISet<BranchInfo> targetBranches)
		{
			MergeChangeset(strChangesetToMerge, sourceBranch, targetBranches, false);
		}

		public ISet<BranchInfo> GetReleasebranches(string strTeamProject)
		{
			string[] items = this.VersionControlAdapter.GetItemsByPath(this.Convention.GetReleaseBranchesPath(strTeamProject));

			ISet<BranchInfo> releaseBranches = new HashSet<BranchInfo>();

			foreach(string item in items)
			{
				BranchInfo branch;
				if(this.Convention.TryGetBranchInfoByServerPath(item, out branch)) releaseBranches.Add(branch);
			}

			return releaseBranches;
		}

		public string MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch)
		{
			return MergeChangeset(strChangesetToMerge, sourceBranch, targetBranch, true);
		}

		public string MergeChangesetWithoutCheckIn(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch)
		{
			return MergeChangeset(strChangesetToMerge, sourceBranch, targetBranch, false);
		}
		#endregion

		#region Privates
		private void MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, IEnumerable<BranchInfo> targetBranches, bool bCheckIn)
		{
			if(strChangesetToMerge == null) throw new ArgumentNullException("strChangesetToMerge");

			bool bSourcebranchMappingCreated = EnsureMapping(sourceBranch);

			foreach(BranchInfo targetBranch in targetBranches)
			{
				MergeChangeset(strChangesetToMerge, sourceBranch, targetBranch, bCheckIn);
			}

			if(bSourcebranchMappingCreated) DeleteMapping(sourceBranch);
		}

		private string MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch, bool bCheckIn)
		{
			if(strChangesetToMerge == null) throw new ArgumentNullException("strChangesetToMerge");

			bool bSourcebranchMappingCreated = EnsureMapping(sourceBranch);
			bool bTargetbranchMappingCreated = EnsureMapping(targetBranch);

			Merge(strChangesetToMerge, sourceBranch, targetBranch);

			string strChangeset = null;

			if(bCheckIn && !this.VersionControlAdapter.HasConflicts(this.Convention.GetServerPath(targetBranch)))
			{
				strChangeset = CheckIn(strChangesetToMerge, targetBranch);

				if(bTargetbranchMappingCreated) DeleteMapping(targetBranch);
			}

			if(bSourcebranchMappingCreated) DeleteMapping(sourceBranch);

			return strChangeset;
		}

		private string CheckIn(string strChangesetToMerge, BranchInfo targetBranch)
		{
			string strComment = this.VersionControlAdapter.GetComment(strChangesetToMerge);
			this.TextOutput.WriteVerbose(string.Format("Checking in {0} with comment {1}", this.Convention.GetServerPath(targetBranch), strComment));

			return this.VersionControlAdapter.CheckIn(this.Convention.GetServerPath(targetBranch), strComment);
		}

		private void Merge(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch)
		{
			this.TextOutput.WriteVerbose(string.Format("Merging changeset {0} from {1} to {2}", strChangesetToMerge, sourceBranch, targetBranch));
			this.VersionControlAdapter.Merge(strChangesetToMerge, this.Convention.GetServerPath(sourceBranch), this.Convention.GetServerPath(targetBranch));
			this.VersionControlAdapter.Get(this.Convention.GetServerPath(targetBranch));
		}

		private bool EnsureMapping(BranchInfo branch)
		{
			string strServerPath = this.Convention.GetServerPath(branch);

			if(!this.VersionControlAdapter.IsServerPathMapped(strServerPath))
			{
				CreateMapping(branch);
				return true;
			}

			this.VersionControlAdapter.Get(strServerPath);

			return false;
		}

		private string GetVersionSpec(BranchInfo branch)
		{
			return string.Format("L{0}", GetLabel(branch));
		}

		private string GetLabel(BranchInfo branch)
		{
			return string.Format("{0}.0", branch.Name);
		}
		#endregion
	}
}