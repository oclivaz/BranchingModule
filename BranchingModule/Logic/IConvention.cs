namespace BranchingModule.Logic
{
	internal interface IConvention
	{
		string GetLocalPath(string strTeamproject, string strBranch);
		string GetServerPath(string strTeamproject, string strBranch);
	}
}
