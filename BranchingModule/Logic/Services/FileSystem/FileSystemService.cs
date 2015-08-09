using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BranchingModule.Logic
{
	internal class FileSystemService : IFileSystemService
	{
		#region Constants
		private const string CMD_EXE = "cmd.exe";
		#endregion

		#region Publics
		public void ExecuteInCmd(string strFile, string strArguments)
		{
			string strCmdarguments = string.Format(@"/C {0} {1}", strFile, strArguments);

			ProcessStartInfo startInfo = new ProcessStartInfo(CMD_EXE)
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

		public string ReadAllText(string strFile)
		{
			return File.ReadAllText(strFile);
		}

		public void WriteAllText(string strFile, string strContent, Encoding encoding)
		{
			string directory = Path.GetDirectoryName(strFile);
			if(directory == null) throw new Exception("Couldn't determine Directory");

			Directory.CreateDirectory(directory);

			File.WriteAllText(strFile, strContent, encoding);
		}

		public void Copy(string strSource, string strDestination)
		{
			File.Copy(strSource, strDestination, true);
		}

		public void Move(string strSource, string strDestination)
		{
			File.Move(strSource, strDestination);
		}

		public void Delete(string strFile)
		{
			File.Delete(strFile);
		}

		public bool Exists(string strFile)
		{
			return File.Exists(strFile);
		}

		public string[] GetFiles(string strDirectory)
		{
			return Directory.GetFiles(strDirectory);
		}
		#endregion
	}
}