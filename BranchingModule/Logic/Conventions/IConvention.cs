﻿namespace BranchingModule.Logic
{
	public interface IConvention
	{
		BranchInfo MainBranch(string teamProject);
		BranchInfo GetBranchInfoByServerPath(string strServerPath);
		BranchType GetBranchType(BranchInfo branch);

		string GetLocalPath(BranchInfo branch);
		string GetServerPath(BranchInfo branch);
		string GetServerBasePath(BranchInfo branch);
		string GetBuildserverDump(BranchInfo branch);
		string GetLocalDump(BranchInfo branch);
		string GetApplicationName(BranchInfo branch);
		string GetSolutionFile(BranchInfo branch);
	}
}
