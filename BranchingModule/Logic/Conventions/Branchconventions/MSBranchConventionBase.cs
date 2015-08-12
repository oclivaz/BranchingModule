using System;

namespace BranchingModule.Logic
{
	internal abstract class MSBranchConventionBase : IBranchConvention
	{
		#region Constants
		protected const string REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)";
		protected const string REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)/([^/]*)";
		#endregion

		#region Publics
		public string GetServerPath(BranchInfo branch)
		{
			return String.Format(@"{0}/Source", GetServerBasePath(branch));
		}

		public abstract BranchType BranchType { get; }

		public string GetLocalPath(BranchInfo branch)
		{
			return String.Format(@"C:\inetpub\wwwroot\{0}", GetApplicationName(branch));
		}

		public string GetSolutionFile(BranchInfo branch)
		{
			return String.Format(@"{0}\{1}.sln", GetLocalPath(branch), branch.TeamProject);
		}

		public abstract string GetServerBasePath(BranchInfo branch);

		public abstract string GetBuildserverDump(BranchInfo branch);

		public abstract string GetLocalDump(BranchInfo branch);

		public abstract string GetApplicationName(BranchInfo branch);

		public abstract bool ServerPathFollowsConvention(string strServerpath);

		public abstract bool BranchnameFollowsConvention(string strBranchname);

		public abstract BranchInfo CreateBranchInfoByServerPath(string strServerpath);
		#endregion
	}
}