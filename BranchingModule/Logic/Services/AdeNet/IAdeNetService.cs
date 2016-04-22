namespace BranchingModule.Logic
{
	public interface IAdeNetService
	{
		void InstallPackages(BranchInfo branch);
		void BuildWebConfig(BranchInfo branch);
		void InitializeIIS(BranchInfo branch);
		void CleanupIIS(BranchInfo branch);
		void CreateBuildDefinition(BranchInfo branch);
		void CreateDatabase(BranchInfo branch);
	}
}
