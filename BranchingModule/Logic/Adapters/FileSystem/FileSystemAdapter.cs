﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using BranchingModule.Logic.Utils;
using ICSharpCode.SharpZipLib.Zip;

namespace BranchingModule.Logic
{
	internal class FileSystemAdapter : IFileSystemAdapter
	{
		#region Publics
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
			File.Delete(strDestination);
			File.Move(strSource, strDestination);
		}

		public void DeleteFile(string strFile)
		{
			if(strFile == null) throw new ArgumentNullException("strFile");

			File.Delete(strFile);
		}

		public void DeleteDirectory(string strDirectory)
		{
			if(strDirectory == null) throw new ArgumentNullException("strDirectory");

			if(Directory.Exists(strDirectory)) Retry.Do(() => Directory.Delete(strDirectory, true), new TimeSpan(0, 0, 0, 0, 200));
		}

		public void CreateDirectory(string strDirectory)
		{
			if(strDirectory == null) throw new ArgumentNullException("strDirectory");

			Directory.CreateDirectory(strDirectory);
		}

		public void EmptyDirectory(string strDirectory)
		{
			if(strDirectory == null) throw new ArgumentNullException("strDirectory");

			Array.ForEach(Directory.GetFiles(strDirectory), File.Delete);
		}

		public bool Exists(string strPath)
		{
			if(strPath == null) throw new ArgumentNullException("strPath");

			return File.Exists(strPath) || Directory.Exists(strPath);
		}

		public IFileInfo[] GetFiles(string strDirectory)
		{
			var dir = new DirectoryInfo(strDirectory);
			return dir.GetFiles().Select(systemFileinfo => (IFileInfo) (new FileInfo(systemFileinfo))).ToArray();
		}

		public IFileInfo GetFileInfo(string strFile)
		{
			return new FileInfo(new System.IO.FileInfo(strFile));
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

			public DateTime ModificationTime
			{
				get { return this.FrameworkFileInfo.LastWriteTime; }
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