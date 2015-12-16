namespace BranchingModule.Logic
{
	public interface ITextOutputService
	{
		void RegisterListener(ITextOutputListener listener);
		void WriteVerbose(string strText);
		void Write(string strText);
	}
}
