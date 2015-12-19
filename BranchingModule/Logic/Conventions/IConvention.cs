namespace BranchingModule.Logic
{
	public interface IConvention
	{
		BranchInfo MainBranch(string strTeamProject);
		BranchInfo GetBranchInfoByServerPath(string strServerPath);
		bool TryGetBranchInfoByServerPath(string strServerPath, out BranchInfo branch);
		BranchType GetBranchType(BranchInfo branch);
		string GetReleaseBranchesPath(string strTeamProject);

		string GetLocalPath(BranchInfo branch);
		string GetServerPath(BranchInfo branch);
		string GetServerBasePath(BranchInfo branch);
		string GetBuildserverDump(BranchInfo branch);
		string GetLocalDump(BranchInfo branch);
		string GetApplicationName(BranchInfo branch);
		string GetSolutionFile(BranchInfo branch);
		string GetAblagePath(BranchInfo branch);
	}
}
