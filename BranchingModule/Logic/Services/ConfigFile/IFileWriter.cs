using System.Text;

namespace BranchingModule.Logic
{
	public interface IFileWriter
	{
		void Write(string strFile, string strContent, Encoding encoding);
	}
}
