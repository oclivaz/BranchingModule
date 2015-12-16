using System.Collections.Generic;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class GetReleasebranchesControllerTest : BranchingModuleTestBase
	{
		#region Fields
		private readonly HashSet<BranchInfo> AKISBV_5_0_35_AND_5_0_40 = new HashSet<BranchInfo>(new[] { AKISBV_5_0_35, AKISBV_5_0_40 });
		#endregion

		#region Properties
		private GetReleasebranchesController GetReleasebranchesController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private ITextOutputService TextOutpt { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.TextOutpt = Substitute.For<ITextOutputService>();
			this.GetReleasebranchesController = new GetReleasebranchesController(this.VersionControl, this.TextOutpt);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetReleasebranches()
		{
			// Arrange
			this.VersionControl.GetReleasebranches(AKISBV).Returns(AKISBV_5_0_35_AND_5_0_40);

			// Act
			this.GetReleasebranchesController.GetReleasebranches(AKISBV);

			// Assert
			this.TextOutpt.Received().Write(Arg.Is<string>(s => s.Contains(AKISBV_5_0_35.Name)));
			this.TextOutpt.Received().Write(Arg.Is<string>(s => s.Contains(AKISBV_5_0_40.Name)));
		}
		#endregion
	}
}