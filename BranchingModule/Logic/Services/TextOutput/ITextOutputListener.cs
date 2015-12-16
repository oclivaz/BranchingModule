namespace BranchingModule.Logic
{
	public interface ITextOutputListener
	{
		void WriteVerbose(string strText);
		void Write(string strText);
	}
}