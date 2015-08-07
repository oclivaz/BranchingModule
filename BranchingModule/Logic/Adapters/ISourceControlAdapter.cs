namespace BranchingModule.Logic
{
	public interface ISourceControlAdapter
	{
		void CreateMapping(string localPath, string serverPath);
	}
}
