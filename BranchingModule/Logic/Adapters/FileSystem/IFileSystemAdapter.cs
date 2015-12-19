using System;
using System.Text;

namespace BranchingModule.Logic
{
	public interface IFileSystemAdapter
	{
		string ReadAllText(string strFile);
		void WriteAllText(string strFile, string strContent, Encoding encoding);

		void Copy(string strSource, string strDestination);
		void Move(string strSource, string strDestination);
		void DeleteFile(string strFile);
		void DeleteDirectory(string strDirectory);
		void CreateDirectory(string strDirectory);
		void EmptyDirectory(string strDirectory);

		bool Exists(string strPath);
		IFileInfo[] GetFiles(string strDirectory);
		IFileInfo GetFileInfo(string strFile);

		void ExtractZip(string strFile, string strTargetDirectory);
	}

	public interface IFileInfo
	{
		string FileName { get; }
		string FullName { get; }
		DateTime CreationTime { get; }
		DateTime ModificationTime { get; }
	}
}