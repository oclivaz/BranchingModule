using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveReleasebranchControllerTest
	{
		#region Constants
		private static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBVBL", "1.2.3");
		#endregion

		#region Properties
		private RemoveReleasebranchController RemoveReleasebranchController { get; set; }
		private ISourceControlService SourceControl { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.SourceControl = Substitute.For<ISourceControlService>();
			this.RemoveReleasebranchController = new RemoveReleasebranchController(this.SourceControl, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRemoveReleasebranch()
		{
			// Act
			this.RemoveReleasebranchController.RemoveReleasebranch(AKISBV_5_0_35);

			// Assert
			this.SourceControl.Received().DeleteBranch(AKISBV_5_0_35);
		}
		#endregion
	}
}