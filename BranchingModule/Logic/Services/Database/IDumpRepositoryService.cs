namespace BranchingModule.Logic
{
	public interface IDumpRepositoryService
	{
		void CopyDump(BranchInfo branch, string strTarget);
	}
}
