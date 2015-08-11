using System;

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
		BranchInfo GetBranchInfo(string strChangeset);
	}
}
