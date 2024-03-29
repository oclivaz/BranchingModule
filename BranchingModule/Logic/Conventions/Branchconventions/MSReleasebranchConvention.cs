﻿using System;
using System.Text.RegularExpressions;

namespace BranchingModule.Logic
{
	internal class MSReleasebranchConvention : MSBranchConventionBase
	{
		#region Constants
		internal const string RELEASE = "Release";

		private const string REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN = @"^[0-9]+\.[0-9]+\.[0-9]+$";
		#endregion

		#region Properties
		public ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MSReleasebranchConvention(ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.Settings = settings;
		}
		#endregion

		#region Publics
		public override BranchType BranchType
		{
			get { return BranchType.Release; }
		}

		public override string GetServerBasePath(BranchInfo branch)
		{
			return String.Format(@"$/{0}/{1}/{2}", branch.TeamProject, RELEASE, branch.Name);
		}

		public override string GetBuildserverDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.BuildServerDumpRepositoryPath
			return String.Format(@"\\build\Backup\{0}_{1}_{2}.bak", branch.TeamProject, RELEASE, branch.Name);
		}

		public override string GetLocalDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.LocalDatabasePath
			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);
			return String.Format(@"c:\Database\{0}\{1}_{2}_{3}.bak", teamProjectSettings.LocalDB, branch.TeamProject, RELEASE, branch.Name);
		}

		public override string GetLocalDatabase(BranchInfo branch)
		{
			return GetApplicationName(branch);
		}

		public override string GetApplicationName(BranchInfo branch)
		{
			return String.Format("{0}_{1}", branch.TeamProject, branch.Name.Replace('.', '_'));
		}

		public override bool ServerPathFollowsConvention(string strServerpath)
		{
			Match matchFolderhierarchyDepth3 = new Regex(REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES).Match(strServerpath);

			if(matchFolderhierarchyDepth3.Success)
			{
				string strBranchType = matchFolderhierarchyDepth3.Groups[2].ToString();
				string branchName = matchFolderhierarchyDepth3.Groups[3].ToString();

				return strBranchType == RELEASE && BranchnameFollowsConvention(branchName);
			}

			return false;
		}

		public override bool BranchnameFollowsConvention(string strBranchname)
		{
			return new Regex(REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN).Match(strBranchname).Success;
		}

		public override BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			Match matchFolderhierarchyDepth3 = new Regex(REGEX_DOLLAR_AND_THREE_GROUPS_BETWEEN_SLASHES).Match(strServerpath);

			if(matchFolderhierarchyDepth3.Success)
			{
				string strTeamproject = matchFolderhierarchyDepth3.Groups[1].ToString();
				string strBranchType = matchFolderhierarchyDepth3.Groups[2].ToString();
				string strBranchName = matchFolderhierarchyDepth3.Groups[3].ToString();

				if(strBranchType == RELEASE && BranchnameFollowsConvention(strBranchName))
				{
					return new BranchInfo(strTeamproject, strBranchName);
				}
			}

			throw new Exception(string.Format("Path {0} does not follow convention", strServerpath));
		}
		#endregion
	}
}