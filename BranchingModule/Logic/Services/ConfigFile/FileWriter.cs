using System;
using System.IO;
using System.Text;

namespace BranchingModule.Logic
{
	internal class FileWriter : IFileWriter
	{
		#region Publics
		public void Write(string strFile, string strContent, Encoding encoding)
		{
			if(strFile == null) throw new ArgumentNullException("strFile");
			if(strContent == null) throw new ArgumentNullException("strContent");
			if(encoding == null) throw new ArgumentNullException("encoding");

			string directory = Path.GetDirectoryName(strFile);
			if(directory == null) throw new Exception("Couldn't determine Directory");

			Directory.CreateDirectory(directory);

			File.WriteAllText(strFile, strContent, Encoding.UTF8);
		}
		#endregion
	}
}