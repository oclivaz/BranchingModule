using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveReleasebranchControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private RemoveReleasebranchController RemoveReleasebranchController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.RemoveReleasebranchController = new RemoveReleasebranchController(this.VersionControl, this.FileSystem, new ConventionDummy(), new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRemoveReleasebranch()
		{
			// Act
			this.RemoveReleasebranchController.RemoveReleasebranch(AKISBV_5_0_35);

			// Assert
			this.VersionControl.Received().DeleteMapping(AKISBV_5_0_35);
			this.VersionControl.Received().DeleteBranch(AKISBV_5_0_35);
			this.FileSystem.Received().DeleteFile(BUILDSERVER_DUMP_AKISBV_5_0_35);
		}
		#endregion
	}
}