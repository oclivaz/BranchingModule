﻿using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSMainbranchConventionTest : BranchingModuleTestBase
	{
		#region Properties
		private IBranchConvention MSMainbranchConvention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.MSMainbranchConvention = new MSMainbranchConvention(this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSMainbranchConvention.GetLocalPath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBVDev", strLocalPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath()
		{
			// Act
			string strServerPath = this.MSMainbranchConvention.GetServerSourcePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath()
		{
			// Act
			string strServerPath = this.MSMainbranchConvention.GetServerBasePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main", strServerPath, true);
		}

		[TestMethod]
		public void TestGetBuildserverDump()
		{
			// Act
			string strBuildserverDump = this.MSMainbranchConvention.GetBuildserverDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"\\build\backup\AkisBV.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetLocalDump()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV_5_0_35.TeamProject).Returns(TeamProjectSettings("TheLocalDBName", "dump"));

			// Act
			string strBuildserverDump = this.MSMainbranchConvention.GetLocalDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\database\TheLocalDBName\dump.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetApplicationName()
		{
			// Act
			string strApplicationName = this.MSMainbranchConvention.GetApplicationName(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual("AkisBVDev", strApplicationName);
		}

		[TestMethod]
		public void TestGetSolutionFile()
		{
			// Act
			string strSolutionFile = this.MSMainbranchConvention.GetSolutionFile(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\inetpub\wwwroot\AkisBVDev\AkisBV.sln", strSolutionFile, true);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_valid_name()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSMainbranchConvention.BranchnameFollowsConvention("Main");

			// Assert
			Assert.IsTrue(bBranchnameFollowsConvention);
		}

		[TestMethod]
		public void TestBranchnameFollowsConvention_with_invalid_name()
		{
			// Act
			bool bBranchnameFollowsConvention = this.MSMainbranchConvention.BranchnameFollowsConvention("2.0.25");

			// Assert
			Assert.IsFalse(bBranchnameFollowsConvention);
		}
		#endregion
	}
}