using System;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class MainbranchConventionDummy : IBranchConvention
	{
		public const string MAIN = "Main";

		#region Properties
		public BranchType BranchType
		{
			get { return BranchType.Main; }
		}
		#endregion

		#region Publics
		public static string GetLocalPath(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetLocalPath {0}", branch);
		}

		string IBranchConvention.GetLocalDatabase(BranchInfo branch)
		{
			return GetLocalDatabase(branch);
		}

		string IBranchConvention.GetServerSourcePath(BranchInfo branch)
		{
			return GetServerPath(branch);
		}

		string IBranchConvention.GetServerBasePath(BranchInfo branch)
		{
			return GetServerBasePath(branch);
		}

		string IBranchConvention.GetBuildserverDump(BranchInfo branch)
		{
			return GetBuildserverDump(branch);
		}

		string IBranchConvention.GetLocalDump(BranchInfo branch)
		{
			return GetLocalDump(branch);
		}

		string IBranchConvention.GetApplicationName(BranchInfo branch)
		{
			return GetApplicationName(branch);
		}

		string IBranchConvention.GetAblagePath(BranchInfo branch)
		{
			return GetAblagePath(branch);
		}

		string IBranchConvention.GetSolutionFile(BranchInfo branch)
		{
			return GetSolutionFile(branch);
		}

		bool IBranchConvention.ServerPathFollowsConvention(string strServerpath)
		{
			return ServerPathFollowsConvention(strServerpath);
		}

		bool IBranchConvention.BranchnameFollowsConvention(string strBranchname)
		{
			return BranchnameFollowsConvention(strBranchname);
		}

		string IBranchConvention.GetLocalPath(BranchInfo branch)
		{
			return GetLocalPath(branch);
		}

		public static string GetLocalDatabase(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetLocalDatabase {0}", branch);
		}

		public static string GetServerPath(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetServerPath {0}", branch);
		}

		public static string GetAblagePath(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetAblagePath {0}", branch);
		}

		public static string GetServerBasePath(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetServerBasePath {0}", branch);
		}

		public static string GetBuildserverDump(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetBuildserverDump {0}", branch);
		}

		public static string GetLocalDump(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetLocalDump {0}", branch);
		}

		public static string GetApplicationName(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetApplicationName {0}", branch);
		}

		public static string GetSolutionFile(BranchInfo branch)
		{
			return string.Format(@"MainbranchConventionDummy.GetSolutionFile {0}", branch);
		}

		public static bool ServerPathFollowsConvention(string strServerpath)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public static bool BranchnameFollowsConvention(string strBranchname)
		{
			return strBranchname == MAIN;
		}

		public BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			return new BranchInfo("MainbranchConventionDummy.MainbranchConventionDummy", "MainbranchConventionDummy");
		}
		#endregion
	}
}