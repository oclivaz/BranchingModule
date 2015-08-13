using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class MergeBugfixControllerText : BranchingModuleTestBase
	{
		private static readonly HashSet<BranchInfo> ANY_BRANCHINFO_SET = new HashSet<BranchInfo>();

		#region Constants
		private const string CHANGESET_123456 = "123456";
		private const string CHANGESET_898989 = "898989";
		#endregion

		#region Properties
		public IDumpService Dump { get; set; }

		private MergeBugfixController MergeBugfixController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.MergeBugfixController = new MergeBugfixController(this.VersionControl, new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestMergeBugfix_changeset_in_Releasebranch()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_5_0_60);
			this.VersionControl.MergeChangeset(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN).Returns(CHANGESET_898989);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name }, false);

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN);
			this.VersionControl.Received().MergeChangeset(CHANGESET_898989, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_Mainbranch()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_MAIN);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name }, false);

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_in_Releasebranch_with_noCheckIn()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_5_0_60);
			this.VersionControl.MergeChangeset(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN).Returns((string) null);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name }, true);

			// Assert
			this.VersionControl.Received().MergeChangesetWithoutCheckIn(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN);
			this.VersionControl.DidNotReceive().MergeChangesetWithoutCheckIn(CHANGESET_898989, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
			this.VersionControl.DidNotReceiveWithAnyArgs().MergeChangeset(DONT_CARE, ANY_BRANCHINFO, ANY_BRANCHINFO_SET);
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_in_Mainbranch_with_noCheckIn()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_MAIN);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name }, true);

			// Assert
			this.VersionControl.Received().MergeChangesetWithoutCheckIn(CHANGESET_123456, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
			this.VersionControl.DidNotReceiveWithAnyArgs().MergeChangeset(DONT_CARE, ANY_BRANCHINFO, ANY_BRANCHINFO_SET);
		}
		#endregion

		#region Privates
		private static HashSet<BranchInfo> SetEquals(IEnumerable<BranchInfo> expectedSet)
		{
			return Arg.Is<HashSet<BranchInfo>>(set => set.SetEquals(expectedSet));
		}
		#endregion
	}
}