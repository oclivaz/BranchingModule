using System;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class MainbranchConventionDummy : IBranchConvention
	{
		#region Constants
		internal const string MAIN = "Main";

		private static readonly string REGEX_MAIN = string.Format(@"^{0}$", MAIN);
		#endregion

		#region Properties
		public BranchType BranchType
		{
			get { return BranchType.Main; }
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
			return String.Format(@"$/{0}/Main", branch.TeamProject);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			// TODO: Introduce ISettings.BuildServerDumpRepositoryPath
			return String.Format(@"\\build\Backup\{0}.bak", branch.TeamProject);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			return String.Format(@"c:\Database\{0}.bak", branch.TeamProject);
		}

		public string GetApplicationName(BranchInfo branch)
		{
			return String.Format("{0}Dev", branch.TeamProject);
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
			return strBranchname == MAIN;
		}

		public BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}