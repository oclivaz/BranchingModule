using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class MergeBugfixControllerText
	{
		#region Constants
		private static readonly BranchInfo AKISBV_1_2_3 = new BranchInfo(AKISBV, "1.2.3");
		private static readonly BranchInfo AKISBV_4_5_6 = new BranchInfo(AKISBV, "4.5.6");
		private static readonly BranchInfo AKISBV_7_8_9 = new BranchInfo(AKISBV, "7.8.9");
		private static readonly BranchInfo AKISBV_MAIN = new BranchInfo(AKISBV, "Main");
		private const string AKISBV = "AkisBV";
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
			this.VersionControl.GetBranchInfo(CHANGESET_123456).Returns(AKISBV_1_2_3);
			this.VersionControl.MergeChangeset(CHANGESET_123456, AKISBV_1_2_3, AKISBV_MAIN).Returns(CHANGESET_898989);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_4_5_6.Name, AKISBV_7_8_9.Name });

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_1_2_3, AKISBV_MAIN);
			this.VersionControl.Received().MergeChangeset(CHANGESET_898989, AKISBV_MAIN, SetEquals(new[] { AKISBV_4_5_6, AKISBV_7_8_9 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_Mainbranch()
		{
			// Arrange
			this.VersionControl.GetBranchInfo(CHANGESET_123456).Returns(AKISBV_MAIN);

			// Act
			this.MergeBugfixController.MergeBugfix(AKISBV, CHANGESET_123456, new[] { AKISBV_4_5_6.Name, AKISBV_7_8_9.Name });

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_MAIN, SetEquals(new[] { AKISBV_4_5_6, AKISBV_7_8_9 }));
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