namespace BranchingModule.Logic
{
	public interface IConvention
	{
		string GetLocalPath(BranchInfo branch);
		string GetServerPath(BranchInfo branch);
		string GetServerBasePath(BranchInfo branch);
		string GetBuildserverDump(BranchInfo branch);
		string GetLocalDump(BranchInfo branch);
		string GetApplicationName(BranchInfo branch);
		BranchInfo GetBranchInfoByServerPath(string strServerItem);
	}
}
