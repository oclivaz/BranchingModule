using System.Diagnostics;

namespace BranchingModule.Logic
{
	internal class FileExecutionService : IFileExecutionService
	{
		public void ExecuteInCmd(string strFile, string strArguments)
		{
			string strCmdarguments = string.Format(@"/C {0} {1}", strFile, strArguments);

			ProcessStartInfo startInfo = new ProcessStartInfo(Executables.CMD_EXE)
			{
				Arguments = strCmdarguments,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			using(Process process = Process.Start(startInfo))
			{
				if(process != null) process.WaitForExit();
			}
		}

		public void Execute(string strFile, string strArguments)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(strFile, strArguments)
			{
				Arguments = strArguments,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			using(Process process = Process.Start(startInfo))
			{
				if(process != null) process.WaitForExit();
			}
		}

		public Process StartProcess(string strFile, string strArguments)
		{
			return Process.Start(strFile, strArguments);
		}
	}
}
