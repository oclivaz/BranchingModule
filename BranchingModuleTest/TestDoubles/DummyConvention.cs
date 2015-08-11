using System;
using System.Text.RegularExpressions;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	public class DummyConvention : IConvention
	{
		#region Constants
		public const string MAINBRANCH_NAME = "Main";

		private const string REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)";
		private const string REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)/([^/]*)";
		private const string REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN = @"^[0-9]\.[0-9]\.[0-9]$";
		#endregion

		#region Publics
		public static BranchInfo CreateDummyMainbranch(string strTeamProject)
		{
			return new BranchInfo(strTeamProject, MAINBRANCH_NAME);
		}

		public string GetLocalPath(BranchInfo branch)
		{
			return String.Format(@"nowhere:\inetpub\wwwroot\{0}", GetApplicationName(branch));
		}

		public string GetServerPath(BranchInfo branch)
		{
			return String.Format(@"{0}/Source", GetServerBasePath(branch));
		}

		public string GetServerBasePath(BranchInfo branch)
		{
			if(branch.Name == MAINBRANCH_NAME) return String.Format(@"$/{0}/Main", branch.TeamProject);
			return String.Format(@"nowhere/{0}/Release/{1}", branch.TeamProject, branch.Name);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			return String.Format(@"\\nowhere\Backup\{0}_Release_{1}.bak", branch.TeamProject, branch.Name);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			return String.Format(@"nowhere:\Database\DummyDatabase\{0}_Release_{1}.bak", branch.TeamProject, branch.Name);
		}

		public string GetApplicationName(BranchInfo branch)
		{
			return String.Format("{0}_{1}", branch.TeamProject, branch.Name.Replace('.', '_'));
		}

		public string GetSolutionFile(BranchInfo branch)
		{
			return String.Format(@"{0}\{1}.sln", GetLocalPath(branch), branch.TeamProject);
		}

		public bool IsReleasebranch(BranchInfo branch)
		{
			return new Regex(REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN).Match(branch.Name).Success;
		}

		public bool IsMainbranch(BranchInfo branch)
		{
			return branch.Name == MAINBRANCH_NAME;
		}

		public BranchInfo MainBranch(string teamProject)
		{
			return CreateDummyMainbranch(teamProject);
		}

		public BranchInfo GetBranchInfoByServerPath(string strServerItem)
		{
			Match matchFolderHierarchyDepth2 = new Regex(REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES).Match(strServerItem);

			if(matchFolderHierarchyDepth2.Success)
			{
				string strTeamProject = matchFolderHierarchyDepth2.Groups[1].ToString();
				string strBranchType = matchFolderHierarchyDepth2.Groups[2].ToString();

				if(strBranchType == MAINBRANCH_NAME) return MainBranch(strTeamProject);

				Match matchFolderhierarchyDepth3 = new Regex(REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES).Match(strServerItem);

				if(matchFolderhierarchyDepth3.Success)
				{
					BranchInfo branch = new BranchInfo(strTeamProject, matchFolderhierarchyDepth3.Groups[3].ToString());
					if(IsReleasebranch(branch)) return branch;
				}
			}

			throw new Exception(String.Format("Could not determine Branch of {0}", strServerItem));
		}
		#endregion
	}
}
