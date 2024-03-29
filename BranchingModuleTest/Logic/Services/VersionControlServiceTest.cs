﻿using System;
using System.Collections.Generic;
using System.Linq;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class VersionControlServiceTest : BranchingModuleTestBase
	{
		#region Constants
		protected const string APPCONFIG_SERVER_PATH = @"£/pathtoappconfig";
		protected const string SERVERITEM = "£/ServerItem";
		protected const string OTHER_SERVERITEM = "£/OtherServerItem";
		protected const string CHANGESETNUMBER = "123456";
		#endregion

		#region Properties
		private IVersionControlService VersionControlService { get; set; }
		private IConvention Convention { get; set; }
		private ITeamFoundationVersionControlAdapter VersionControlAdapter { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = new ConventionDummy();
			this.VersionControlAdapter = Substitute.For<ITeamFoundationVersionControlAdapter>();
			this.VersionControlService = new TeamFoundationService(this.VersionControlAdapter, this.Convention, new TextOutputServiceDummy());
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

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

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

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

			// Act
			versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestCreateMapping()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);

			// Act
			this.VersionControlService.CreateMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_35, LOCAL_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestCreateMapping_with_invalid_server_path()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(false);

			// Act
			ExceptionAssert.Throws<ArgumentException>(() => this.VersionControlService.CreateMapping(AKISBV_5_0_35), "does not exist");
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
		public void TestGetCreationTime_with_Releasebranch()
		{
			// Act
			this.VersionControlService.GetCreationTime(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().GetCreationTime(Arg.Any<string>(), Arg.Any<string>());
		}

		[TestMethod]
		public void TestGetCreationTime_with_development_branch()
		{
			// Act
			this.VersionControlService.GetCreationTime(AKISBV_STD_10);

			// Assert
			this.VersionControlAdapter.Received().GetLatestCheckingDate(SERVER_PATH_AKISBV_STD_10);
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
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().CreateBranch(ANY_STRING, ANY_STRING, ANY_STRING);
		}

		[TestMethod]
		public void TestGetBranchInfoByChangeset()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();

			convention.GetBranchInfoByServerPath(ANY_STRING).ReturnsForAnyArgs(AKISBV_5_0_35);
			this.VersionControlAdapter.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVER_PATH_AKISBV_5_0_35 });

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

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

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

			// Act
			versionControlService.GetBranchInfoByChangeset(CHANGESETNUMBER);
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(ANY_STRING).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasPendingChanges(SERVER_PATH_AKISBV_MAIN).Returns(false);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().CreateMapping(ANY_STRING, ANY_STRING);
			Received.InOrder(() =>
			                 {
				                 this.VersionControlAdapter.Get(SERVER_PATH_AKISBV_MAIN);
				                 this.VersionControlAdapter.Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
				                 this.VersionControlAdapter.Get(SERVER_PATH_AKISBV_MAIN); // Leider nötig
				                 this.VersionControlAdapter.CheckIn(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
			                 }
				);

			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().CreateMapping(ANY_STRING, ANY_STRING);
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().DeleteMapping(ANY_STRING, ANY_STRING);
		}

		[TestMethod]
		public void TestMergeChangeset_both_branches_mapped_with_conflict()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(ANY_STRING).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().Undo(ANY_STRING);
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().DeleteMapping(ANY_STRING, ANY_STRING);
		}

		[TestMethod]
		public void TestMergeChangeset_no_branches_mapped_without_conflict()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.IsServerPathMapped(ANY_STRING).ReturnsForAnyArgs(false);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN);

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_5_0_35, SERVER_PATH_AKISBV_MAIN);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_MAIN, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_multiple_targetbranches()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(ANY_STRING).ReturnsForAnyArgs(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_MAIN).Returns(false);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_40).Returns(true);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_MAIN, new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40 });

			// Assert
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_40);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_5_0_40, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_multiple_targetbranches_no_mappings_one_conflict()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_40).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);
			this.VersionControlAdapter.IsServerPathMapped(ANY_STRING).ReturnsForAnyArgs(false);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.HasConflicts(SERVER_PATH_AKISBV_5_0_40).Returns(false);

			// Act
			this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_MAIN, new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40 });

			// Assert
			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.DidNotReceive().CheckIn(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());
			this.VersionControlAdapter.DidNotReceive().Undo(SERVER_PATH_AKISBV_5_0_35);
			this.VersionControlAdapter.DidNotReceive().DeleteMapping(SERVER_PATH_AKISBV_5_0_35, Arg.Any<string>());

			this.VersionControlAdapter.Received().CreateMapping(SERVER_PATH_AKISBV_5_0_40, Arg.Any<string>());
			this.VersionControlAdapter.Received().Merge(CHANGESETNUMBER, SERVER_PATH_AKISBV_MAIN, SERVER_PATH_AKISBV_5_0_40);
			this.VersionControlAdapter.Received().CheckIn(SERVER_PATH_AKISBV_5_0_40, Arg.Any<string>());
			this.VersionControlAdapter.Received().DeleteMapping(SERVER_PATH_AKISBV_5_0_40, Arg.Any<string>());
		}

		[TestMethod]
		public void TestMergeChangeset_PendingChanges_on_Targetbranch()
		{
			// Arrange
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_5_0_35).Returns(true);
			this.VersionControlAdapter.ServerItemExists(SERVER_PATH_AKISBV_MAIN).Returns(true);

			this.VersionControlAdapter.HasPendingChanges(SERVER_PATH_AKISBV_MAIN).Returns(true);

			// Act
			ExceptionAssert.Throws<ArgumentException>(() => this.VersionControlService.MergeChangeset(CHANGESETNUMBER, AKISBV_5_0_35, AKISBV_MAIN), "has pending Changes");
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
			this.VersionControlAdapter.DidNotReceiveWithAnyArgs().DeleteBranch(ANY_STRING);
		}

		[TestMethod]
		public void TestGetReleasebranches_valid_branch()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();
			convention.GetReleaseBranchesPath(AKISBV).Returns(SERVERITEM);
			this.VersionControlAdapter.GetItemsByPath(convention.GetReleaseBranchesPath(AKISBV)).Returns(new[] { SERVER_BASEPATH_AKISBV_5_0_35 });

			BranchInfo branch;
			convention.TryGetBranchInfoByServerPath(SERVER_BASEPATH_AKISBV_5_0_35, out branch).Returns(BranchInfoOut(AKISBV_5_0_35));

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

			// Act
			ISet<BranchInfo> branches = versionControlService.GetReleasebranches(AKISBV);

			// Assert
			CollectionAssert.AreEqual(new[] { AKISBV_5_0_35 }, branches.ToArray());
		}

		[TestMethod]
		public void TestGetReleasebranches_invalid_branch()
		{
			// Arrange
			IConvention convention = Substitute.For<IConvention>();
			convention.GetReleaseBranchesPath(AKISBV).Returns(SERVERITEM);
			this.VersionControlAdapter.GetItemsByPath(convention.GetReleaseBranchesPath(AKISBV)).Returns(new[] { SERVER_BASEPATH_AKISBV_5_0_35 });

			BranchInfo branch;
			convention.TryGetBranchInfoByServerPath(SERVER_BASEPATH_AKISBV_5_0_35, out branch).Returns(NoSuccess());

			IVersionControlService versionControlService = new TeamFoundationService(this.VersionControlAdapter, convention, new TextOutputServiceDummy());

			// Act
			ISet<BranchInfo> branches = versionControlService.GetReleasebranches(AKISBV);

			// Assert
			Assert.AreEqual(0, branches.Count);
		}

		[TestMethod]
		public void TestGetLatest()
		{
			// Act
			this.VersionControlService.GetLatest(AKISBV_5_0_35);

			// Assert
			this.VersionControlAdapter.Received().Get(SERVER_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetChangesetComment()
		{
			// Arrange
			this.VersionControlAdapter.GetComment(CHANGESETNUMBER).Returns(COMMENT);

			// Act
			string strComment = this.VersionControlService.GetChangesetComment(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(COMMENT, strComment);
		}

		[TestMethod]
		public void TestIsMapped_with_mapping()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(SERVER_PATH_AKISBV_5_0_35).Returns(true);

			// Act
			bool bMapped = this.VersionControlService.IsMapped(AKISBV_5_0_35);

			// Assert
			Assert.IsTrue(bMapped);
		}

		[TestMethod]
		public void TestIsMapped_without_mapping()
		{
			// Arrange
			this.VersionControlAdapter.IsServerPathMapped(SERVER_PATH_AKISBV_5_0_35).Returns(false);

			// Act
			bool bMapped = this.VersionControlService.IsMapped(AKISBV_5_0_35);

			// Assert
			Assert.IsFalse(bMapped);
		}

		[TestMethod]
		public void TestDownloadFile()
		{
			// Act
			this.VersionControlService.DownloadFile("serverPath", "localPath");

			// Assert
			this.VersionControlAdapter.Received().DownloadFile("serverPath", "localPath");
		}
		#endregion

		#region Privates
		private static Func<CallInfo, bool> BranchInfoOut(BranchInfo branch)
		{
			return x =>
			       {
				       x[1] = branch;
				       return true;
			       };
		}

		private static Func<CallInfo, bool> NoSuccess()
		{
			return x =>
			       {
				       x[1] = new BranchInfo();
				       return false;
			       };
		}
		#endregion
	}
}