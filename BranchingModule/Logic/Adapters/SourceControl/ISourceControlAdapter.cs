namespace BranchingModule.Logic
{
	public interface ISourceControlAdapter
	{
		void CreateMapping(BranchInfo branch);
		void CreateAppConfig(BranchInfo branch);
	}
}
