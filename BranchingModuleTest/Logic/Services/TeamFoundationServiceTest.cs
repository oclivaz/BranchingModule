﻿using System;
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
		private static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo(AKISBV, "5.0.35");
		private static readonly BranchInfo AKISBV_5_0_40 = new BranchInfo(AKISBV, "5.0.40");
		private static readonly BranchInfo AKISBV_MAIN = ConventionDummy.MainBranch(AKISBV);
		private const string DONT_CARE = "";
		private static readonly string LOCAL_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetLocalPath(AKISBV_5_0_35);
		private static readonly string SERVER_PATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetServerPath(AKISBV_5_0_35);
		private static readonly string SERVER_BASEPATH_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetServerBasePath(AKISBV_5_0_35);
		private static readonly string SERVER_PATH_AKISBV_5_0_40 = ReleasebranchConventionDummy.GetServerPath(AKISBV_5_0_40);
		private static readonly string SERVER_PATH_AKISBV_MAIN = MainbranchConventionDummy.GetServerPath(AKISBV_MAIN);
		private const string APPCONFIG_SERVER_PATH = @"£/pathtoappconfig";
		private const string SERVERITEM = "£/ServerItem";
		private const string OTHER_SERVERITEM = "£/OtherServerItem";
		private const string CHANGESETNUMBER = "123456";
		private const string AKISBV = "AkisBV";

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
			this.Convention = new ConventionDummy();
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
			IConvention convention = Substitute.For<IConvention>();

			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });
			convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_MAIN);

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, this.Settings, new TextOutputServiceDummy());

			// Act
			BranchInfo branch = versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestGetBranchInfo_multiple_branches()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();

			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });
			convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_5_0_35);

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, this.Settings, new TextOutputServiceDummy());

			// Act
			versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestCreateMapping()
		{
			// Act
			this.VersionControlService.CreateMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_35, LOCAL_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestDeleteMapping()
		{
			// Act
			this.VersionControlService.DeleteMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_5_0_35, LOCAL_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().Get();
		}

		[TestMethod]
		public void TestCreateAppConfig()
		{
			// Arrange
			this.Settings.AppConfigServerPath.Returns(APPCONFIG_SERVER_PATH);

			// Act
			this.VersionControlService.CreateAppConfig(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DownloadFile(APPCONFIG_SERVER_PATH, string.Format(@"{0}\Web\app.config", LOCAL_PATH_AKISBV_5_0_35));
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
			// Act
			this.VersionControlService.CreateBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().CreateBranch(SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_35, "L5.0.35.0");
		}

		[TestMethod]
		public void TestCreateBranch_branch_already_exists()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);

			// Act
			this.VersionControlService.CreateBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().CreateBranch(DONT_CARE, DONT_CARE, DONT_CARE);
		}

		[TestMethod]
		public void TestGetBranchInfoByChangeset()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();

			convention.GetBranchInfoByServerPath(DONT_CARE).ReturnsForAnyArgs(AKISBV_5_0_35);
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVER_PATH_AKISBV_5_0_35 });
			
			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, this.Settings, new TextOutputServiceDummy());

			// Act
			BranchInfo branch = versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(AKISBV_5_0_35, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestGetBranchInfoByChangeset_multiple_branches()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();

			convention.GetBranchInfoByServerPath(SERVER_PATH_AKISBV_5_0_35).Returns(AKISBV_5_0_35);
			convention.GetBranchInfoByServerPath(SERVER_PATH_AKISBV_MAIN).Returns(AKISBV_MAIN);
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN });

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, this.Settings, new TextOutputServiceDummy());

			// Act
			versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_with_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(true);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().Undo(SERVER_PATH_AKISBV_MAIN);
		}

		[TestMethod]
		public void TestMergeChangeset_no_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(false);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_multiple_targetbranches()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(DONT_CARE).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_MAIN, new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40 });

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_40);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_5_0_40, Arg.Any<string>());
		}

		[TestMethod]
		public void TestDeleteBranch()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_BASEPATH_AKISBV_5_0_35).Returns(true);

			// Act
			this.VersionControlService.DeleteBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().DeleteBranch(SERVER_BASEPATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestDeleteBranch_already_deleted()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_BASEPATH_AKISBV_5_0_35).Returns(false);

			// Act
			this.VersionControlService.DeleteBranch(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().DeleteBranch(DONT_CARE);
		}
		#endregion
	}
}