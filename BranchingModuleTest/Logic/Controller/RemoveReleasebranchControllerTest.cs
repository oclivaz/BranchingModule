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
		private const string BUILDSERVER_DUMP = @"\\build\somwhere\dump.bak";
		#endregion

		#region Properties
		private RemoveReleasebranchController RemoveReleasebranchController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.Convention = Substitute.For<IConvention>();
			this.RemoveReleasebranchController = new RemoveReleasebranchController(this.VersionControl, this.FileSystem, this.Convention, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRemoveReleasebranch()
		{
			// Arrange
			this.Convention.GetBuildserverDump(AKISBV_5_0_35).Returns(BUILDSERVER_DUMP);

			// Act
			this.RemoveReleasebranchController.RemoveReleasebranch(AKISBV_5_0_35);

			// Assert
			this.VersionControl.Received().DeleteMapping(AKISBV_5_0_35);
			this.VersionControl.Received().DeleteBranch(AKISBV_5_0_35);
			this.FileSystem.Received().DeleteFile(BUILDSERVER_DUMP);
		}
		#endregion
	}
}