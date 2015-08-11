using System;
using System.Text.RegularExpressions;

namespace BranchingModule.Logic
{
	internal class MSConvention : IConvention
	{
		#region Constants
		private const string REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)";
		private const string REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES = @"^\$/([^/]*)/([^/]*)/([^/]*)";
		private const string REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN = @"^[0-9]\.[0-9]\.[0-9]$";
		#endregion

		#region Properties
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MSConvention(ISettings settings)
		{
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public string GetLocalPath(BranchInfo branch)
		{
			return string.Format(@"C:\inetpub\wwwroot\{0}", GetApplicationName(branch));
		}

		public string GetServerPath(BranchInfo branch)
		{
			return string.Format(@"{0}/Source", GetServerBasePath(branch));
		}

		public string GetServerBasePath(BranchInfo branch)
		{
			if(branch.Name == BranchInfo.MAIN) return string.Format(@"$/{0}/Main", branch.TeamProject);
			return string.Format(@"$/{0}/Release/{1}", branch.TeamProject, branch.Name);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			return string.Format(@"\\build\Backup\{0}_Release_{1}.bak", branch.TeamProject, branch.Name);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);
			return string.Format(@"c:\Database\{0}\{1}_Release_{2}.bak", teamProjectSettings.LocalDB, branch.TeamProject, branch.Name);
		}

		public string GetApplicationName(BranchInfo branch)
		{
			return string.Format("{0}_{1}", branch.TeamProject, branch.Name.Replace('.', '_'));
		}

		public BranchInfo GetBranchInfoByServerPath(string strServerItem)
		{
			Match matchFolderHierarchyDepth2 = new Regex(REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES).Match(strServerItem);

			if(matchFolderHierarchyDepth2.Success)
			{
				string strTeamProject = matchFolderHierarchyDepth2.Groups[1].ToString();
				string strBranchType = matchFolderHierarchyDepth2.Groups[2].ToString();

				if(strBranchType == BranchInfo.MAIN) return BranchInfo.Main(strTeamProject);

				Match matchFolderhierarchyDepth3 = new Regex(REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES).Match(strServerItem);

				if(matchFolderhierarchyDepth3.Success)
				{
					Match releaseMatch = new Regex(REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN).Match(matchFolderhierarchyDepth3.Groups[3].ToString());
					if(releaseMatch.Success) return new BranchInfo(strTeamProject, matchFolderhierarchyDepth3.Groups[3].ToString());
				}
			}

			throw new Exception(string.Format("Could not determine Branch of {0}", strServerItem));
		}

		public string GetSolutionFile(BranchInfo branch)
		{
			return string.Format(@"{0}\{1}.sln", GetLocalPath(branch), branch.TeamProject);
		}
		#endregion
	}
}