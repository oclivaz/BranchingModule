namespace BranchingModule.Logic
{
	public interface ITextOutputListener
	{
		void WriteVerbose(string strText);
		void WriteLine(string strText);
	}
}