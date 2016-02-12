namespace BranchingModule.Logic
{
	public interface IConfigFileService
	{
		void CreateIndivConfig(BranchInfo branch);
		void CreateAppConfig(BranchInfo branch);
	}
}
