namespace BranchingModule.Logic
{
	public interface IAdeNetAdapter
	{
		void InstallPackages(BranchInfo branch);
		void BuildWebConfig(BranchInfo branch);
	}
}
