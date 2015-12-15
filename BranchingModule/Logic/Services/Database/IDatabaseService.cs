namespace BranchingModule.Logic
{
	public interface IDatabaseService
	{
		void Backup(BranchInfo branch);
		void Restore(BranchInfo branch);
		void InstallBuildserverDump(BranchInfo branch);
		void DeleteLocalDump(BranchInfo branch);
	}
}
