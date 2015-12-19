using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSConventionTest : BranchingModuleTestBase
	{
		#region Properties
		private MSConvention MSConvention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.MSConvention = new MSConvention(this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSConvention.GetLocalPath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_5_0_35", strLocalPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath_Release()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/5.0.35/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath_Main()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath(AKISBV_MAIN);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath_Release()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerBasePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/5.0.35", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath_Main()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerBasePath(AKISBV_MAIN);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main", strServerPath, true);
		}

		[TestMethod]
		public void TestGetBuildserverDump()
		{
			// Act
			string strBuildserverDump = this.MSConvention.GetBuildserverDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"\\build\backup\AkisBV_Release_5.0.35.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetLocalDump()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV_5_0_35.TeamProject).Returns(TeamProjectSettings("TheLocalDBName", "whatever"));

			// Act
			string strBuildserverDump = this.MSConvention.GetLocalDump(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\database\TheLocalDBName\AkisBV_Release_5.0.35.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetApplicationName()
		{
			// Act
			string strApplicationName = this.MSConvention.GetApplicationName(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual("AkisBV_5_0_35", strApplicationName);
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Releasebranch()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Release/5.0.35/Something/that/doesnt.matter");

			// Assert
			Assert.AreEqual(AKISBV_5_0_35, branch);
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Developmentbranch()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Development/Std-10/Something/that/doesnt.matter");

			// Assert
			Assert.AreEqual(AKISBV_STD_10, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void TestGetBranchInfoByServerItem_invalid_Releasebranch_without_number()
		{
			// Act
			this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Release");
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void TestGetBranchInfoByServerItem_invalid_Releasebranch()
		{
			// Act
			this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Release/1.5z.7/Something/that/doesnt.matter");
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Mainbranch()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Main/Source/whatever/whatever");

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Mainbranch_with_minimal_Information()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Main");

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void TestGetBranchInfoByServerItem_Unknown_Branch()
		{
			// Act
			this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/SomethingElse/Source");
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_Developmentbranch()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Development/Std-10/Something/that/doesnt.matter", out branch);

			// Assert
			Assert.IsTrue(bSuccess);
			Assert.AreEqual(AKISBV_STD_10, branch);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_Releasebranch()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Release/1.5.7/Something/that/doesnt.matter", out branch);

			// Assert
			Assert.IsTrue(bSuccess);
			Assert.AreEqual(CreateBranchInfo("AkisBV", "1.5.7"), branch);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_invalid_Releasebranch_without_number()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Release", out branch);

			// Assert
			Assert.IsFalse(bSuccess);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_invalid_Releasebranch()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Release/1.5z.7/Something/that/doesnt.matter", out branch);

			// Assert
			Assert.IsFalse(bSuccess);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_Mainbranch()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Main/Source/whatever/whatever", out branch);

			// Assert
			Assert.IsTrue(bSuccess);
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_Mainbranch_with_minimal_Information()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/Main", out branch);

			// Assert
			Assert.IsTrue(bSuccess);
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		public void TestTryGetBranchInfoByServerItem_Unknown_Branch()
		{
			// Arrange
			BranchInfo branch;

			// Act
			bool bSuccess = this.MSConvention.TryGetBranchInfoByServerPath("$/AkisBV/SomethingElse/Source", out branch);

			// Assert
			Assert.IsFalse(bSuccess);
		}

		[TestMethod]
		public void TestGetSolutionFile()
		{
			// Act
			string strSolutionFile = this.MSConvention.GetSolutionFile(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\inetpub\wwwroot\AkisBV_5_0_35\AkisBV.sln", strSolutionFile, true);
		}

		[TestMethod]
		public void TestGetReleaseBranchesPath()
		{
			// Act
			string strReleaseBranchesPath = this.MSConvention.GetReleaseBranchesPath(AKISBV);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release", strReleaseBranchesPath);
		}

		[TestMethod]
		public void TestGetAblagePath_Release()
		{
			// Act
			string strAblagePath = this.MSConvention.GetAblagePath(AKISBV_5_0_35);

			// Assert
			Assert.AreEqual(@"c:\temp\ablage\AkisBV_5_0_35", strAblagePath, true);
		}

		[TestMethod]
		public void TestGetAblagePath_Main()
		{
			// Act
			string strAblagePath = this.MSConvention.GetAblagePath(AKISBV_MAIN);

			// Assert
			Assert.AreEqual(@"c:\temp\ablage\AkisBVDev", strAblagePath, true);
		}
		#endregion

		#region Privates
		private BranchInfo CreateBranchInfo(string strTeamproject, string strName)
		{
			return new BranchInfo(strTeamproject, strName);
		}
		#endregion
	}
}