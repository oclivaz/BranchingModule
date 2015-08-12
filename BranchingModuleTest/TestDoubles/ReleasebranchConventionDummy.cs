using System;
using System.Text.RegularExpressions;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class ReleasebranchConventionDummy : IBranchConvention
	{
		#region Constants
		private const string RELEASE = "Release";

		private const string REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN = @"^[0-9]\.[0-9]\.[0-9]$";
		#endregion

		#region Properties
		public BranchType BranchType
		{
			get { return BranchType.Release; }
		}
		#endregion

		#region Publics
		public string GetLocalPath(BranchInfo branch)
		{
			throw new NotImplementedException();
		}

		public string GetServerPath(BranchInfo branch)
		{
			throw new NotImplementedException();
		}

		public string GetServerBasePath(BranchInfo branch)
		{
			return String.Format(@"$/{0}/{1}/{2}", branch.TeamProject, RELEASE, branch.Name);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.BuildServerDumpRepositoryPath
			return String.Format(@"\\build\Backup\{0}_{1}_{2}.bak", branch.TeamProject, RELEASE, branch.Name);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.LocalDatabasePath
			return String.Format(@"c:\Database\{0}{1}.bak", branch.TeamProject, branch.Name);
		}

		public string GetApplicationName(BranchInfo branch)
		{
			return String.Format("{0}_{1}", branch.TeamProject, branch.Name.Replace('.', '_'));
		}

		public string GetSolutionFile(BranchInfo branch)
		{
			throw new NotImplementedException();
		}

		public bool ServerPathFollowsConvention(string strServerpath)
		{
			throw new NotImplementedException();
		}

		public bool BranchnameFollowsConvention(string strBranchname)
		{
			return new Regex(REGEX_THREE_NUMBERS_WITH_PERIOD_IN_BETWEEN).Match(strBranchname).Success;
		}

		public BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}