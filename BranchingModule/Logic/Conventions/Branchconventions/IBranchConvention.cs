namespace BranchingModule.Logic
{
	public interface IBranchConvention
	{
		BranchType BranchType { get; }

		string GetLocalPath(BranchInfo branch);
		string GetLocalDatabase(BranchInfo branch);
		string GetServerSourcePath(BranchInfo branch);
		string GetServerBasePath(BranchInfo branch);
		string GetBuildserverDump(BranchInfo branch);
		string GetLocalDump(BranchInfo branch);
		string GetApplicationName(BranchInfo branch);
		string GetAblagePath(BranchInfo branch);
		string GetSolutionFile(BranchInfo branch);

		bool ServerPathFollowsConvention(string strServerpath);
		bool BranchnameFollowsConvention(string strBranchname);

		BranchInfo CreateBranchInfoByServerPath(string strServerpath);
	}
}
