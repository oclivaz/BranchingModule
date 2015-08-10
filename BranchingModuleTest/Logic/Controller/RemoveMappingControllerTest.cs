using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveMappingControllerTest
	{
		#region Constants
		private static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBVBL", "1.2.3");
		private const string LOCAL_PATH = @"c:\AkisBV";
		#endregion

		#region Properties
		private RemoveMappingController RemoveMappingController { get; set; }
		private ISourceControlService SourceControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IFileSystemService FileSystem { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.SourceControl = Substitute.For<ISourceControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.FileSystem = Substitute.For<IFileSystemService>();
			this.Convention = Substitute.For<IConvention>();

			this.RemoveMappingController = new RemoveMappingController(this.SourceControl, this.AdeNet, this.FileSystem, this.Convention, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestProcess()
		{
			// Arrange
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(LOCAL_PATH);

			// Act
			this.RemoveMappingController.RemoveMapping(AKISBV_5_0_35);

			// Assert
			this.SourceControl.Received().DeleteMapping(AKISBV_5_0_35);
			this.FileSystem.Received().DeleteDirectory(LOCAL_PATH);
			this.AdeNet.Received().RemoveApplication(AKISBV_5_0_35);
		}
		#endregion
	}
}