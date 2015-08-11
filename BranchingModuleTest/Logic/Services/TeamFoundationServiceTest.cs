using System;
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
		private static readonly BranchInfo AKISBV_MAIN = new BranchInfo("AkisBV", "Main");
		private const string SERVERITEM = "$/ServerItem";
		private const string OTHER_SERVERITEM = "$/OtherServerItem";
		private const string CHANGESETNUMBER = "123456";

		#region Properties
		private IVersionControlService VersionControlService { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		private ITeamFoundationVersionControlAdapter VersionControl { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.VersionControl = Substitute.For<ITeamFoundationVersionControlAdapter>();
			this.VersionControlService = new TeamFoundationService(this.VersionControl, this.Convention, this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetBranchInfo_single_branch()
		{
			// Arrange
			this.VersionControl.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });

			this.Convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			this.Convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_MAIN);

			// Act
			BranchInfo branch = this.VersionControlService.GetBranchInfo(CHANGESETNUMBER);

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestGetBranchInfo_multiple_branches()
		{
			// Arrange
			this.VersionControl.GetServerItemsByChangeset(CHANGESETNUMBER).Returns(new[] { SERVERITEM, OTHER_SERVERITEM });

			this.Convention.GetBranchInfoByServerPath(SERVERITEM).Returns(AKISBV_MAIN);
			this.Convention.GetBranchInfoByServerPath(OTHER_SERVERITEM).Returns(AKISBV_5_0_35);

			// Act
			this.VersionControlService.GetBranchInfo(CHANGESETNUMBER);
		}
		#endregion
	}
}