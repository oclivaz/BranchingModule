using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class ShowReleasebranchesControllerTest : BranchingModuleTestBase
	{
		#region Fields
		private readonly HashSet<BranchInfo> AKISBV_5_0_35_AND_5_0_40 = new HashSet<BranchInfo>(new[] { AKISBV_5_0_35, AKISBV_5_0_40 });
		#endregion

		#region Properties
		private ShowReleasebranchesController ShowReleasebranchesController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private ITextOutputService TextOutpt { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.TextOutpt = Substitute.For<ITextOutputService>();
			this.ShowReleasebranchesController = new ShowReleasebranchesController(this.VersionControl, this.TextOutpt);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestShowReleasebranchestwo_branches_one_mapped()
		{
			// Arrange
			this.VersionControl.GetReleasebranches(AKISBV).Returns(AKISBV_5_0_35_AND_5_0_40);
			this.VersionControl.IsMapped(AKISBV_5_0_35).Returns(true);

			// Act
			this.ShowReleasebranchesController.ShowReleasebranches(AKISBV);

			// Assert
			this.TextOutpt.Received().Write(Arg.Is<string>(s => s.Contains(AKISBV_5_0_35.Name) && s.Contains("mapped")));
			this.TextOutpt.Received().Write(Arg.Is<string>(s => s.Contains(AKISBV_5_0_40.Name) && !s.Contains("mapped")));
		}
		#endregion
	}
}