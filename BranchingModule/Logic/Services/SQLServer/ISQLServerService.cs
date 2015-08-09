namespace BranchingModule.Logic
{
	public interface ISQLServerService
	{
		void ExecuteScript(string strScript, string strDB);
	}
}