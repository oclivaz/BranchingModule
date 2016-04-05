using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;

namespace BranchingModuleTest.TestDoubles
{
	internal class DevelopmentbranchConventionDummy : IBranchConvention
	{
		#region Properties
		public BranchType BranchType
		{
			get { return BranchType.Development; }
		}
		#endregion

		#region Publics
		public static string GetLocalPath(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetLocalPath {0}", branch);
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

		BranchInfo IBranchConvention.CreateBranchInfoByServerPath(string strServerpath)
		{
			return CreateBranchInfoByServerPath(strServerpath);
		}

		string IBranchConvention.GetLocalPath(BranchInfo branch)
		{
			return GetLocalPath(branch);
		}

		public static string GetServerPath(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetServerPath {0}", branch);
		}

		public static string GetAblagePath(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetAblagePath {0}", branch);
		}

		public static string GetServerBasePath(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetServerBasePath/{0}/{1}", branch.TeamProject, branch.Name);
		}

		public static string GetBuildserverDump(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetBuildserverDump {0}", branch);
		}

		public static string GetLocalDump(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetLocalDump {0}", branch);
		}

		public static string GetApplicationName(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetApplicationName {0}", branch);
		}

		public static string GetSolutionFile(BranchInfo branch)
		{
			return string.Format(@"DevelopmentbranchConventionDummy.GetSolutionFile {0}", branch);
		}

		public static bool ServerPathFollowsConvention(string strServerpath)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public static bool BranchnameFollowsConvention(string strBranchname)
		{
			return strBranchname == BranchingModuleTestBase.AKISBV_STD_10.Name;
		}

		public static BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			return new BranchInfo("DevelopmentbranchConventionDummy", "DevelopmentbranchConventionDummy");
		}
		#endregion
	}
}