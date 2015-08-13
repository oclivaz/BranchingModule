using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSReleasebranchConventionTest : BranchingModuleTestBase
	{
		#region Properties
		private IBranchConvention MSReleasebranchConvention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.MSReleasebranchConvention = new MSReleasebranchConvention(this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSReleasebranchConvention.GetLocalPath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_5_0_35", strLocalPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath()
		{
			// Act
			string strServerPath = this.MSReleasebranchConvention.GetServerSourcePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/5.0.35/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath()
		{
			// Act
			string strServerPath = this.MSReleasebranchConvention.GetServerBasePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/5.0.35", strServerPath, true);
		}

		[TestMethod]
		public void TestGetBuildserverDump()
		{
			// Act
			string strBuildserverDump = this.MSReleasebranchConvention.GetBuildserverDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"\\build\backup\AkisBV_Release_5.0.35.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetLocalDump()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV_5_0_35.TeamProject).Returns(TeamProjectSettings("TheLocalDBName", "whatever"));

			// Act
			string strBuildserverDump = this.MSReleasebranchConvention.GetLocalDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\database\TheLocalDBName\AkisBV_Release_5.0.35.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetApplicationName()
		{
			// Act
			string strApplicationName = this.MSReleasebranchConvention.GetApplicationName(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual("AkisBV_5_0_35", strApplicationName);
		}

		[TestMethod]
		public void TestGetSolutionFile()
		{
			// Act
			string strSolutionFile = this.MSReleasebranchConvention.GetSolutionFile(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\inetpub\wwwroot\AkisBV_5_0_35\AkisBV.sln", strSolutionFile, true);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_valid_name()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSReleasebranchConvention.BranchnameFollowsConvention("2.0.25");

			// Assert
			Assert.IsTrue(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_invalid_name_two_dots()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSReleasebranchConvention.BranchnameFollowsConvention("2..25");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_invalid_name_too_many_numbers()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSReleasebranchConvention.BranchnameFollowsConvention("2.0.25.0");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_invalid_name_wich_characters()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSReleasebranchConvention.BranchnameFollowsConvention("2.5e.25");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}
		#endregion
	}
}