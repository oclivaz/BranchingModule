namespace BranchingModule.Logic
{
	public interface IDatabaseService
	{
		void Create(BranchInfo branch);

		void Backup(BranchInfo branch);
		void Backup(BranchInfo branch, string strFile);

		void Restore(BranchInfo branch);
		void Restore(BranchInfo branch, string strFile);

		void InstallBuildserverDump(BranchInfo branch);
		void DeleteLocalDump(BranchInfo branch);
	}
}