namespace BranchingModule.Logic
{
	public interface IDumpService
	{
		void RestoreDump(BranchInfo branch);
		void InstallBuildserverDump(BranchInfo branch);
		void DeleteLocalDump(BranchInfo branch);
	}
}
