using System;
using System.Collections.Generic;

namespace BranchingModule.Logic
{
	public interface IVersionControlService
	{
		void CreateMapping(BranchInfo branch);
		void DeleteMapping(BranchInfo branch);
		void CreateAppConfig(BranchInfo branch);
		DateTime GetCreationTime(BranchInfo branch);
		void CreateBranch(BranchInfo branch);
		void DeleteBranch(BranchInfo branch);
		BranchInfo GetBranchInfoByChangeset(string strChangeset);
		string MergeChangesetWithoutCheckIn(string strChangesetToMerge, BranchInfo sourceBranch, BranchInfo targetBranch);
		string MergeChangeset(string strChangeset, BranchInfo sourceBranch, BranchInfo targetBranch);
		void MergeChangeset(string strChangesetToMerge, BranchInfo sourceBranch, ISet<BranchInfo> targetBranches);
		void MergeChangesetWithoutCheckIn(string strChangesetToMerge, BranchInfo sourceBranch, ISet<BranchInfo> targetBranches);
	}
}
