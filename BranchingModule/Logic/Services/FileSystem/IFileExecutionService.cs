using System.Diagnostics;

namespace BranchingModule.Logic
{
	public interface IFileExecutionService
	{
		void ExecuteInCmd(string strFile, string strArguments);
		void Execute(string strFile, string strArguments);
		Process StartProcess(string strFile, string strArguments);
	}
}
