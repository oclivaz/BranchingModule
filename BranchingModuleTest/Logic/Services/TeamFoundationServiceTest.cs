using System;
using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class TeamFoundationServiceTest : BranchingModuleTestBase
	{
		private static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		private static readonly BranchInfo AKISBV_5_0_40 = new BranchInfo("AkisBV", "5.0.40");
		private static readonly BranchInfo AKISBV_MAIN = new BranchInfo("AkisBV", "Main");
		private const string DONT_CARE = "";
		private const string LOCAL_PATH = @"c:\temp";
		private const string SERVER_PATH = @"£\AkisBV";
		private const string SERVER_PATH_2 = @"£\AkisBV\2";
		private const string SERVER_PATH_MAIN = @"£\AkisBV\Main";
		private const string APPCONFIG_SERVER_PATH = @"£/pathtoappconfig";
		private const string SERVERITEM = "£/ServerItem";
		private const string OTHER_SERVERITEM = "£/OtherServerItem";
		private const string CHANGESETNUMBER = "123456";

		#region Properties
		private IVersionControlService VersionControlService { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		private ITeamFoundationVersionControlAdapter VersionControlAdapter { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.VersionControlAdapter = Substitute.For<ITeamFoundationVersionControlAdapter>();
			this.VersionControlService = new TeamFoundationService(this.VersionControlAdapter, this.Convention, this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetBranchInfo_single_branch()
		{
			// Arrange
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });

			this.Convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			this.Convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_MAIN);

			// Act
			BranchInfo branch = this.VersionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestGetBranchInfo_multiple_branches()
		{
			// Arrange
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });

			this.Convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			this.Convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_5_0_35);

			// Act
			this.VersionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestCreateMapping()
		{
			// Arrange
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(LOCAL_PATH);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);

			// Act
			this.VersionControlService.CreateMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH, LOCAL_PATH);
			this.VersionControlAdapter.Received().Get(SERVER_PATH);
		}

		[TestMethod]
		public void TestDeleteMapping()
		{
			// Arrange
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(LOCAL_PATH);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);

			// Act
			this.VersionControlService.DeleteMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH, LOCAL_PATH);
			this.VersionControlAdapter.Received().Get();
		}

		[TestMethod]
		public void TestCreateAppConfig()
		{
			// Arrange
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(LOCAL_PATH);
			this.Settings.AppConfigServerPath.Returns(APPCONFIG_SERVER_PATH);

			// Act
			this.VersionControlService.CreateAppConfig(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DownloadFile(APPCONFIG_SERVER_PATH, string.Format(@"{0}\Web\app.config", LOCAL_PATH));
		}

		[TestMethod]
		public void TestGetCreationTime()
		{
			// Act
			this.VersionControlService.GetCreationTime(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().GetCreationTime(Arg.Any<string>(), Arg.Any<string>());
		}

		[TestMethod]
		public void TestCreateBranch()
		{
			// Arrange
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.Convention.MainBranch(AKISBV_5_0_35.TeamProject).Returns(AKISBV_MAIN);
			this.Convention.GetServerPath(AKISBV_MAIN).Returns(SERVER_PATH_MAIN);

			// Act
			this.VersionControlService.CreateBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().CreateBranch(SERVER_PATH_MAIN, SERVER_PATH, "L5.0.35.0");
		}

		[TestMethod]
		public void TestCreateBranch_branch_already_exists()
		{
			// Arrange
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH).Returns(true);

			// Act
			this.VersionControlService.CreateBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().CreateBranch(DONT_CARE, DONT_CARE, DONT_CARE);
		}

		[TestMethod]
		public void TestGetBranchInfoByChangeset()
		{
			// Arrange
			this.Convention.GetBranchInfoByServerPath(DONT_CARE).ReturnsForAnyArgs(AKISBV_5_0_35);
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVER_PATH });

			// Act
			BranchInfo branch = this.VersionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(AKISBV_5_0_35, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestGetBranchInfoByChangeset_multiple_branches()
		{
			// Arrange
			this.Convention.GetBranchInfoByServerPath(SERVER_PATH).Returns(AKISBV_5_0_35);
			this.Convention.GetBranchInfoByServerPath(SERVER_PATH_MAIN).Returns(AKISBV_MAIN);
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVER_PATH, SERVER_PATH_MAIN });

			// Act
			this.VersionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.Convention.GetServerPath(AKISBV_MAIN).Returns(SERVER_PATH_MAIN);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH, SERVER_PATH_MAIN);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_MAIN, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_with_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.Convention.GetServerPath(AKISBV_MAIN).Returns(SERVER_PATH_MAIN);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_MAIN).Returns(true);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH, SERVER_PATH_MAIN);
			this.VersionControlAdapter.Received().Undo(SERVER_PATH_MAIN);
		}

		[TestMethod]
		public void TestMergeChangeset_no_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(false);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.Convention.GetServerPath(AKISBV_MAIN).Returns(SERVER_PATH_MAIN);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH, Arg.Any<string>());
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH, SERVER_PATH_MAIN);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_MAIN, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_multiple_targetbranches()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.Convention.GetServerPath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.Convention.GetServerPath(AKISBV_5_0_40).Returns(SERVER_PATH_2);
			this.Convention.GetServerPath(AKISBV_MAIN).Returns(SERVER_PATH_MAIN);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_MAIN, new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40 });

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_MAIN, SERVER_PATH);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_MAIN, SERVER_PATH_2);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_2, Arg.Any<string>());
		}

		[TestMethod]
		public void TestDeleteBranch()
		{
			// Arrange
			this.Convention.GetServerBasePath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH).Returns(true);

			// Act
			this.VersionControlService.DeleteBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DeleteBranch(SERVER_PATH);
		}

		[TestMethod]
		public void TestDeleteBranch_already_deleted()
		{
			// Arrange
			this.Convention.GetServerBasePath(AKISBV_5_0_35).Returns(SERVER_PATH);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH).Returns(false);

			// Act
			this.VersionControlService.DeleteBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().DeleteBranch(DONT_CARE);
		}
		#endregion
	}
}