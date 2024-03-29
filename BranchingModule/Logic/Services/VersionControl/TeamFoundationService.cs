﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class TeamFoundationService : IVersionControlService
	{
		#region Properties
		private ITextOutputService TextOutput { get; set; }

		private IConvention Convention { get; set; }

		private ITeamFoundationVersionControlAdapter VersionControlAdapter { get; set; }
		#endregion

		#region Constructors
		public TeamFoundationService(ITeamFoundationVersionControlAdapter versoControlAdapterAdapter, IConvention convention, ITextOutputService textOutputService)
		{
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControlAdapter = versoControlAdapterAdapter;
			this.Convention = convention;
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

		public bool IsMapped(BranchInfo branch)
		{
			return this.VersionControlAdapter.IsServerPathMapped(this.Convention.GetServerPath(branch));
		}

		public DateTime GetCreationTime(BranchInfo branch)
		{
			if(this.Convention.GetBranchType(branch) == BranchType.Release)
			{
				string strNuspecItem = string.Format(@"{0}/{1}.nuspec", this.Convention.GetServerPath(this.Convention.MainBranch(branch.TeamProject)), branch.TeamProject);
				string strVersionSpec = GetVersionSpec(branch);

				return this.VersionControlAdapter.GetCreationTime(strNuspecItem, strVersionSpec);
			}

			string strSourceFolderItem = this.Convention.GetServerPath(branch);
			return this.VersionControlAdapter.GetLatestCheckingDate(strSourceFolderItem);
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

		public void GetLatest(BranchInfo branch)
		{
			this.VersionControlAdapter.Get(this.Convention.GetServerPath(branch));
		}

		public string GetChangesetComment(string strChangeset)
		{
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");

			return this.VersionControlAdapter.GetComment(strChangeset);
		}

		public void DownloadFile(string strServerPath, string strLocalPath)
		{
			if(strServerPath == null) throw new ArgumentNullException("strServerPath");
			if(strLocalPath == null) throw new ArgumentNullException("strLocalPath");

			this.VersionControlAdapter.DownloadFile(strServerPath, strLocalPath);
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
			if(!this.VersionControlAdapter.ServerItemExists(this.Convention.GetServerPath(sourceBranch))) throw new ArgumentException(string.Format("Serverpath {0} for {1} does not exist.", this.Convention.GetServerPath(sourceBranch), sourceBranch));
			if(!this.VersionControlAdapter.ServerItemExists(this.Convention.GetServerPath(targetBranch))) throw new ArgumentException(string.Format("Serverpath {0} for {1} does not exist.", this.Convention.GetServerPath(targetBranch), targetBranch));
			if(this.VersionControlAdapter.HasPendingChanges(this.Convention.GetServerPath(targetBranch))) throw new ArgumentException(string.Format("Serverpath {0} for {1} has pending Changes.", this.Convention.GetServerPath(targetBranch), targetBranch));

			bool bSourcebranchMappingCreated = EnsureMapping(sourceBranch);
			bool bTargetbranchMappingCreated = EnsureMapping(targetBranch);

			Merge(strChangesetToMerge, sourceBranch, targetBranch);

			string strChangeset = null;

			if(!this.VersionControlAdapter.HasConflicts(this.Convention.GetServerPath(targetBranch)))
			{
				strChangeset = CheckIn(strChangesetToMerge, targetBranch);

				if(bTargetbranchMappingCreated) DeleteMapping(targetBranch);
			}

			if(bSourcebranchMappingCreated) DeleteMapping(sourceBranch);

			return strChangeset;
		}
		#endregion

		#region Privates
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