namespace BranchingModule.Logic
{
	public interface ISQLServerAdapter
	{
		void ExecuteScript(string strScript, string strDB);
	}
}