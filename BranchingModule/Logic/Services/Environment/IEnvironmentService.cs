namespace BranchingModule.Logic
{
	public interface IEnvironmentService
	{
		void OpenSolution(BranchInfo branch);
		void OpenWeb(BranchInfo branch);
		void ResetLocalWebserver();
	}
}