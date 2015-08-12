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
			if(strChangesetToMerge == null) throw new ArgumentNullException("strChangesetToMerge");

			bool bSourcebranchMappingCreated = EnsureMapping(sourceBranch);

			foreach(BranchInfo targetBranch in targetBranches)
			{
				MergeChangeset(strChangesetToMerge, sourceBranch, targetBranch);
			}

			if(bSourcebranchMappingCreated) DeleteMapping(sourceBranch);
		}

		public string MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch)
		{
			if(strChangesetToMerge == null) throw new ArgumentNullException("strChangesetToMerge");

			bool bSourcebranchMappingCreated = EnsureMapping(sourceBranch);
			bool bTargetbranchMappingCreated = EnsureMapping(targetBranch);

			this.VersionControlAdapter.Merge(strChangesetToMerge, this.Convention.GetServerPath(sourceBranch), this.Convention.GetServerPath(targetBranch));

			string strChangeset = null;

			if(this.VersionControlAdapter.HasConflicts(this.Convention.GetServerPath(targetBranch)))
			{
				this.VersionControlAdapter.Undo(this.Convention.GetServerPath(targetBranch));
			}
			else
			{
				strChangeset = this.VersionControlAdapter.CheckIn(this.Convention.GetServerPath(targetBranch), this.VersionControlAdapter.GetComment(strChangesetToMerge));
			}

			if(bTargetbranchMappingCreated) DeleteMapping(targetBranch);
			if(bSourcebranchMappingCreated) DeleteMapping(sourceBranch);

			return strChangeset;
		}
		#endregion

		#region Privates
		private bool EnsureMapping(BranchInfo branch)
		{
			string strServerPath = this.Convention.GetServerPath(branch);

			if(this.VersionControlAdapter.IsServerPathMapped(strServerPath)) return false;

			this.TextOutput.WriteVerbose(string.Format("Mapping {0}", strServerPath));
			this.VersionControlAdapter.CreateMapping(strServerPath, this.Convention.GetLocalPath(branch));

			return true;
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