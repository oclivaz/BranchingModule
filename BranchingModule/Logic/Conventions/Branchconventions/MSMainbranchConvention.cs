using System;
using System.Text.RegularExpressions;

namespace BranchingModule.Logic
{
	internal class MSMainbranchConvention : MSBranchConventionBase
	{
		#region Constants
		internal const string MAIN = "Main";
		#endregion

		#region Properties
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MSMainbranchConvention(ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.Settings = settings;
		}
		#endregion

		#region Publics
		public override BranchType BranchType
		{
			get { return BranchType.Main; }
		}

		public override string GetServerBasePath(BranchInfo branch)
		{
			return String.Format(@"$/{0}/Main", branch.TeamProject);
		}

		public override string GetBuildserverDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.BuildServerDumpRepositoryPath
			return String.Format(@"\\build\Backup\{0}.bak", branch.TeamProject);
		}

		public override string GetLocalDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.LocalDatabasePath
			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);
			return String.Format(@"c:\Database\{0}\{1}.bak", teamProjectSettings.LocalDB, branch.TeamProject);
		}

		public override string GetApplicationName(BranchInfo branch)
		{
			return String.Format("{0}Dev", branch.TeamProject);
		}

		public override bool ServerPathFollowsConvention(string strServerpath)
		{
			Match matchFolderHierarchyDepth2 = new Regex(REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES).Match(strServerpath);

			return matchFolderHierarchyDepth2.Success && matchFolderHierarchyDepth2.Groups[2].ToString() == MAIN;
		}

		public override bool BranchnameFollowsConvention(string strBranchname)
		{
			return strBranchname == MAIN;
		}

		public override BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			Match matchFolderHierarchyDepth2 = new Regex(REGEX_DOLLAR_AND_TWO_GROUPS_BETWEEN_SLASHES).Match(strServerpath);
			if(matchFolderHierarchyDepth2.Success)
			{
				string strTeamproject = matchFolderHierarchyDepth2.Groups[1].ToString();
				string strBranchType = matchFolderHierarchyDepth2.Groups[2].ToString();

				if(strBranchType == MAIN) return new BranchInfo(strTeamproject, MAIN);
			}

			throw new Exception(string.Format("Path {0} does not follow convention", strServerpath));
		}
		#endregion
	}
}