using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSDevelpmentBranchConventionTest : BranchingModuleTestBase
	{
		#region Properties
		private IBranchConvention MSDevelopmentBranchConvention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.MSDevelopmentBranchConvention = new MSDevelopmentBranchConvention(this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSDevelopmentBranchConvention.GetLocalPath(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_Std-10", strLocalPath, true);
		}

		[TestMethod]
		public void TestGetLocalDatabase()
		{
			// Act
			string strLocalDatabase = this.MSDevelopmentBranchConvention.GetLocalDatabase(AKISBV_STD_10);

			// Assert
			Assert.AreEqual("AkisBV_Std-10", strLocalDatabase);
		}

		[TestMethod]
		public void TestGetSeverPath()
		{
			// Act
			string strServerPath = this.MSDevelopmentBranchConvention.GetServerSourcePath(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Development/Std-10/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath()
		{
			// Act
			string strServerPath = this.MSDevelopmentBranchConvention.GetServerBasePath(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Development/Std-10", strServerPath, true);
		}

		[TestMethod]
		public void TestGetBuildserverDump()
		{
			// Act
			string strBuildserverDump = this.MSDevelopmentBranchConvention.GetBuildserverDump(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"\\build\backup\AkisBV_Development_Std-10.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetLocalDump()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV_STD_10.TeamProject).Returns(TeamProjectSettings("TheLocalDBName", "whatever"));

			// Act
			string strBuildserverDump = this.MSDevelopmentBranchConvention.GetLocalDump(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"c:\database\TheLocalDBName\AkisBV_Development_Std-10.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetApplicationName()
		{
			// Act
			string strApplicationName = this.MSDevelopmentBranchConvention.GetApplicationName(AKISBV_STD_10);

			// Assert
			Assert.AreEqual("AkisBV_Std-10", strApplicationName);
		}

		[TestMethod]
		public void TestGetSolutionFile()
		{
			// Act
			string strSolutionFile = this.MSDevelopmentBranchConvention.GetSolutionFile(AKISBV_STD_10);

			// Assert
			Assert.AreEqual(@"c:\inetpub\wwwroot\AkisBV_Std-10\AkisBV.sln", strSolutionFile, true);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_valid_name()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSDevelopmentBranchConvention.BranchnameFollowsConvention("Std-10");

			// Assert
			Assert.IsTrue(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_Releasebranch()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSDevelopmentBranchConvention.BranchnameFollowsConvention("2.5.25");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_Mainbranch()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSDevelopmentBranchConvention.BranchnameFollowsConvention("Main");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}
		#endregion
	}
}