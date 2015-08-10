using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace BranchingModule.Logic
{
	internal class FileSystemService : IFileSystemService
	{
		#region Publics
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

		public void DeleteFile(string strFile)
		{
			File.Delete(strFile);
		}

		public void DeleteDirectory(string strDirectory)
		{
			if(Directory.Exists(strDirectory)) Directory.Delete(strDirectory, true);
		}

		public bool Exists(string strFile)
		{
			return File.Exists(strFile);
		}

		public IFileInfo[] GetFiles(string strDirectory)
		{
			var dir = new DirectoryInfo(strDirectory);
			return dir.GetFiles().Select(systemFileinfo => (IFileInfo) (new FileInfo(systemFileinfo))).ToArray();
		}

		public void ExtractZip(string strFile, string strTargetDirectory)
		{
			FastZip fastZip = new FastZip();
			fastZip.ExtractZip(strFile, strTargetDirectory, null);
		}
		#endregion

		#region Struct FileInfo
		private struct FileInfo : IFileInfo
		{
			#region Properties
			public string FileName
			{
				get { return this.FrameworkFileInfo.Name; }
			}

			public string FullName
			{
				get { return this.FrameworkFileInfo.FullName; }
			}

			public DateTime CreationTime
			{
				get { return this.FrameworkFileInfo.CreationTime; }
			}

			private System.IO.FileInfo FrameworkFileInfo { get; set; }
			#endregion

			#region Constructors
			public FileInfo(System.IO.FileInfo fileInfo) : this()
			{
				this.FrameworkFileInfo = fileInfo;
			}
			#endregion
		}
		#endregion
	}
}