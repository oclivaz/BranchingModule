namespace BranchingModule.Logic
{
	public interface IConvention
	{
		string GetLocalPath(BranchInfo branch);
		string GetBuildserverPath(BranchInfo branch);
		string GetBuildserverDump(BranchInfo branch);
		string GetLocalDump(BranchInfo branch);
	}
}
