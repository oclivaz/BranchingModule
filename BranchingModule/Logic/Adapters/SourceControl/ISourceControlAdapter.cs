namespace BranchingModule.Logic
{
	public interface ISourceControlAdapter
	{
		void CreateMapping(BranchInfo branch);
		void DownloadFile(string strServerpath, string strLocalpath);
	}
}
