﻿using System;
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
		#region Constants
		private const string CHANGESET_123456 = "123456";
		private const string CHANGESET_123456_COMMENT = "123456_COMMENT";
		private const string CHANGESET_898989 = "898989";
		#endregion

		#region Properties
		private MergeBugfixController MergeBugfixController { get; set; }

		private IVersionControlService VersionControl { get; set; }
		private IUserInputService UserInput { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.UserInput = Substitute.For<IUserInputService>();

			this.UserInput.RequestConfirmation(Arg.Any<string>()).Returns(true);

			this.MergeBugfixController = new MergeBugfixController(this.VersionControl, this.UserInput, new SettingsDummy(), new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestMergeBugfix_changeset_in_Releasebranch()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_5_0_60);
			this.VersionControl.MergeChangeset(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN).Returns(CHANGESET_898989);
			this.VersionControl.GetChangesetComment(CHANGESET_123456).Returns(CHANGESET_123456_COMMENT);

			// Act
			this.MergeBugfixController.MergeBugfix(CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name });

			// Assert
			this.UserInput.Received().RequestConfirmation(Arg.Is<string>(s => s.Contains(CHANGESET_123456) && s.Contains(CHANGESET_123456_COMMENT)));
			this.VersionControl.Received().GetChangesetComment(CHANGESET_123456);
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_5_0_60, AKISBV_MAIN);
			this.VersionControl.Received().MergeChangeset(CHANGESET_898989, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_Mainbranch()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_MAIN);
			this.VersionControl.GetChangesetComment(CHANGESET_123456).Returns(CHANGESET_123456_COMMENT);

			// Act
			this.MergeBugfixController.MergeBugfix(CHANGESET_123456, new[] { AKISBV_5_0_35.Name, AKISBV_5_0_40.Name });

			// Assert
			this.UserInput.Received().RequestConfirmation(Arg.Is<string>(s => s.Contains(CHANGESET_123456) && s.Contains(CHANGESET_123456_COMMENT)));
			this.VersionControl.Received().GetChangesetComment(CHANGESET_123456);
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_in_Mainbranch_with_no_targetbranches()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_MAIN);
			this.VersionControl.GetReleasebranches(AKISBV).Returns(new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40 });

			// Act
			this.MergeBugfixController.MergeBugfix(CHANGESET_123456, new string[0]);

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_123456, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_35, AKISBV_5_0_40 }));
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_in_Releasebranch_with_no_targetbranches()
		{
			// Arrange
			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_5_0_35);
			this.VersionControl.GetReleasebranches(AKISBV).Returns(new HashSet<BranchInfo> { AKISBV_5_0_35, AKISBV_5_0_40, AKISBV_5_0_60 });
			this.VersionControl.MergeChangeset(CHANGESET_123456, AKISBV_5_0_35, AKISBV_MAIN).Returns(CHANGESET_898989);

			// Act
			this.MergeBugfixController.MergeBugfix(CHANGESET_123456, new string[0]);

			// Assert
			this.VersionControl.Received().MergeChangeset(CHANGESET_898989, AKISBV_MAIN, SetEquals(new[] { AKISBV_5_0_40, AKISBV_5_0_60 }));
		}

		[TestMethod]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestMergeBugfix_with_not_supported_TeamProject()
		{
			// Arrange
			ISettings settings = Substitute.For<ISettings>();

			this.VersionControl.GetBranchInfoByChangeset(CHANGESET_123456).Returns(AKISBV_MAIN);
			settings.IsSupportedTeamproject(AKISBV).Returns(false);

			MergeBugfixController mergeBugfixController = new MergeBugfixController(this.VersionControl, new UserInputServiceDummy(), settings, new ConventionDummy());

			// Act
			mergeBugfixController.MergeBugfix(CHANGESET_123456, new[] { AKISBV_5_0_35.Name });
		}

		[TestMethod]
		public void TestMergeBugfix_changeset_in_Releasebranch_no_confirmation()
		{
			// Arrange
			IUserInputService userInput = Substitute.For<IUserInputService>();
			userInput.RequestConfirmation(CHANGESET_123456).Returns(false);

			MergeBugfixController mergeBugfixController = new MergeBugfixController(this.VersionControl, userInput, new SettingsDummy(), new ConventionDummy());

			// Act
			mergeBugfixController.MergeBugfix(CHANGESET_123456, new[] { AKISBV_5_0_35.Name });

			// Assert
			this.VersionControl.DidNotReceiveWithAnyArgs().MergeChangeset(ANY_STRING, ANY_BRANCH, ANY_BRANCH);
		}
		#endregion
	}
}