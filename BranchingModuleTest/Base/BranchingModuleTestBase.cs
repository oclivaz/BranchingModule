using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Base
{
	[TestClass]
	public class BranchingModuleTestBase
	{
		#region Constants
		protected const string AKISBV = "AkisBV";
		protected static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo(AKISBV, "5.0.35");
		protected static readonly BranchInfo AKISBV_5_0_40 = new BranchInfo(AKISBV, "5.0.40");
		protected static readonly BranchInfo AKISBV_5_0_60 = new BranchInfo(AKISBV, "1.2.3");
		protected static readonly BranchInfo AKISBV_MAIN = ConventionDummy.MainBranch(AKISBV);
		protected static readonly BranchInfo ANY_BRANCHINFO = new BranchInfo(DONT_CARE, DONT_CARE);
		protected static readonly string LOCAL_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetLocalPath(AKISBV_5_0_35);
		protected static readonly string LOCAL_SOLUTION_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetLocalPath(AKISBV_5_0_35);
		protected static readonly string LOCAL_SOLUTION_FILE_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetSolutionFile(AKISBV_5_0_35);
		protected static readonly string SERVER_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetServerPath(AKISBV_5_0_35);
		protected static readonly string SERVER_BASEPATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetServerBasePath(AKISBV_5_0_35);
		protected static readonly string SERVER_BASEPATH_AKISBV_5_0_40 = ReleasebranchConventionDummy.GetServerBasePath(AKISBV_5_0_40);
		protected static readonly string SERVER_PATH_AKISBV_5_0_40 = ReleasebranchConventionDummy.GetServerPath(AKISBV_5_0_40);
		protected static readonly string SERVER_PATH_AKISBV_MAIN = MainbranchConventionDummy.GetServerPath(AKISBV_MAIN);
		protected static readonly string BUILDSERVER_DUMP_AKISBV_5_0_35	 = ReleasebranchConventionDummy.GetBuildserverDump(AKISBV_5_0_35);
		protected const string DONT_CARE = "";
		#endregion

		#region Protecteds
		protected ITeamProjectSettings TeamProjectSettings(string strLocalDB, string strRefDB)
		{
			TeamProjectSettingsDTO dto = new TeamProjectSettingsDTO
			                             {
				                             LocalDB = strLocalDB,
				                             RefDB = strRefDB
			                             };

			return new TeamProjectSettings(dto);
		}

		protected static HashSet<BranchInfo> SetEquals(IEnumerable<BranchInfo> expectedSet)
		{
			return Arg.Is<HashSet<BranchInfo>>(set => set.SetEquals(expectedSet));
		}
		#endregion
	}
}