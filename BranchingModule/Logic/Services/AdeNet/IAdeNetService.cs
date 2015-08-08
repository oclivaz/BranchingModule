namespace BranchingModule.Logic
{
	public interface IAdeNetService
	{
		void InstallPackages(BranchInfo branch);
		void BuildWebConfig(BranchInfo branch);
		void InitializeIIS(BranchInfo branch);
	}
}
