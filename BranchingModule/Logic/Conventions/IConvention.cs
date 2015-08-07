namespace BranchingModule.Logic
{
	public interface IConvention
	{
		string GetLocalPath(BranchInfo branch);
		string GetServerPath(BranchInfo branch);
	}
}
