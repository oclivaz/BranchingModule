﻿using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSConventionTest : BranchingModuleTestBase
	{
		#region Constants
		private static readonly BranchInfo AKISBV_2_5_3 = new BranchInfo("AkisBV", "2.5.3");
		#endregion

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
			string strLocalPath = this.MSConvention.GetLocalPath(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_2_5_3", strLocalPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath_Release()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/2.5.3/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverPath_Main()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath(BranchInfo.Main("AkisBV"));

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main/Source", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath_Release()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerBasePath(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/2.5.3", strServerPath, true);
		}

		[TestMethod]
		public void TestGetSeverBasePath_Main()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerBasePath(BranchInfo.Main("AkisBV"));

			// Assert
			Assert.AreEqual(@"$/AkisBV/Main", strServerPath, true);
		}

		[TestMethod]
		public void TestGetBuildserverDump()
		{
			// Act
			string strBuildserverDump = this.MSConvention.GetBuildserverDump(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual(@"\\build\backup\AkisBV_Release_2.5.3.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetLocalDump()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV_2_5_3.TeamProject).Returns(TeamProjectSettings("TheLocalDBName", "whatever"));

			// Act
			string strBuildserverDump = this.MSConvention.GetLocalDump(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual(@"c:\database\TheLocalDBName\AkisBV_Release_2.5.3.bak", strBuildserverDump, true);
		}

		[TestMethod]
		public void TestGetApplicationName()
		{
			// Act
			string strApplicationName = this.MSConvention.GetApplicationName(AKISBV_2_5_3);

			// Assert
			Assert.AreEqual("AkisBV_2_5_3", strApplicationName);
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Releasebranch()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Release/1.5.7/Something/that/doesnt.matter");

			// Assert
			Assert.AreEqual(CreateBranchInfo("AkisBV", "1.5.7"), branch);
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
		[ExpectedException(typeof(Exception))]
		public void TestGetBranchInfoByServerItem_Featurebranch()
		{
			// Act
			this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Development/SomeName_1/Something/that/doesnt.matter");
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Mainbranch()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Main/Source/whatever/whatever");

			// Assert
			Assert.AreEqual(BranchInfo.Main("AkisBV"), branch);
		}

		[TestMethod]
		public void TestGetBranchInfoByServerItem_Mainbranch_with_minimal_Information()
		{
			// Act
			BranchInfo branch = this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/Main");

			// Assert
			Assert.AreEqual(BranchInfo.Main("AkisBV"), branch);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void TestGetBranchInfoByServerItem_Unknown_Branch()
		{
			// Act
			this.MSConvention.GetBranchInfoByServerPath("$/AkisBV/SomethingElse/Source");
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