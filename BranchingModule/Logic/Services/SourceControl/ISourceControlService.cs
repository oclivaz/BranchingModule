namespace BranchingModule.Logic
{
	public interface ISourceControlService
	{
		void CreateMapping(BranchInfo branch);
		void CreateAppConfig(BranchInfo branch);
	}
}
