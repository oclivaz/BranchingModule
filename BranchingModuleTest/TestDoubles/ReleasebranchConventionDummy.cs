using System;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class ReleasebranchConventionDummy : IBranchConvention
	{
		#region Properties
		public BranchType BranchType
		{
			get { return BranchType.Release; }
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
			return string.Format(@"ReleasebranchConventionDummy.GetServerPath {0}", branch);
		}

		public static string GetServerBasePath(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetServerBasePath/{0}/{1}", branch.TeamProject, branch.Name);
		}

		public static string GetBuildserverDump(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetBuildserverDump {0}", branch);
		}

		public static string GetLocalDump(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetLocalDump {0}", branch);
		}

		public static string GetApplicationName(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetApplicationName {0}", branch);
		}

		public static string GetSolutionFile(BranchInfo branch)
		{
			return string.Format(@"ReleasebranchConventionDummy.GetSolutionFile {0}", branch);
		}

		public static bool ServerPathFollowsConvention(string strServerpath)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public static bool BranchnameFollowsConvention(string strBranchname)
		{
			return strBranchname != MainbranchConventionDummy.MAIN;
		}

		public static BranchInfo CreateBranchInfoByServerPath(string strServerpath)
		{
			return new BranchInfo("ReleasebranchConventionDummy", "ReleasebranchConventionDummy");
		}
		#endregion
	}
}