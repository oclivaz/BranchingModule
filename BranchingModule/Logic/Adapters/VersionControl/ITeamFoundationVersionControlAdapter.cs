using System;

namespace BranchingModule.Logic
{
	public interface ITeamFoundationVersionControlAdapter
	{
		void DeleteMapping(string strServerPath, string strLocalPath);
		void CreateMapping(string strServerPath, string strLocalPath);

		void CreateBranch(string strSourceBranch, string strTargetBranch, string strVersionSpec);
		void DeleteBranch(string strBranchBasePath);

		void Get(string strServerPath);
		void Get();

		bool ServerItemExists(string strServerItem);
		DateTime GetCreationTime(string strItem, string strVersionSpec);
		string[] GetServerItemsByChangeset(string strChangeset);
	}
}
