using System;

namespace BranchingModule.Logic
{
	public interface ITeamFoundationVersionControlAdapter
	{
		void DeleteMapping(string strServerPath, string strLocalPath);
		void CreateMapping(string strServerPath, string strLocalPath);
		bool IsServerPathMapped(string strServerPath);

		void CreateBranch(string strSourceBranch, string strTargetBranch, string strVersionSpec);
		void DeleteBranch(string strBranchBasePath);

		void Get(string strServerPath);
		void Get();

		bool ServerItemExists(string strServerItem);
		DateTime GetCreationTime(string strItem, string strVersionSpec);
		string[] GetItemsByPath(string strServerPath);
		string[] GetServerItemsByChangeset(string strChangeset);

		void Merge(string strChangeset, string strSourcePath, string strTargetPath);
		bool HasConflicts(string strServerPath);
		void Undo(string strServerPath);
		string CheckIn(string strServerPath, string strComment);
		string GetComment(string strChangeset);

		void DownloadFile(string strServerpath, string strLocalpath);
	}
}
