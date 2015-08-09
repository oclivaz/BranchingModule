using System.Text;

namespace BranchingModule.Logic
{
	public interface IFileSystemService
	{
		void ExecuteInCmd(string strFile, string strArguments);
		void Execute(string strFile, string strArguments);
		string ReadAllText(string strFile);
		void WriteAllText(string strFile, string strContent, Encoding encoding);
		void Copy(string strSource, string strDestination);
		void Move(string strSource, string strDestination);
		void Delete(string strFile);
		bool Exists(string strFile);
		string[] GetFiles(string strDirectory);
	}
}