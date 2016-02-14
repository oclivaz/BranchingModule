using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveMappingControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private RemoveMappingController RemoveMappingController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		private IEnvironmentService Environment { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();
			this.Environment = Substitute.For<IEnvironmentService>();

			this.RemoveMappingController = new RemoveMappingController(this.VersionControl, this.AdeNet, this.FileSystem, this.Database, this.Ablage, this.Environment,
			                                                           new ConventionDummy(), new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRemoveMapping()
		{
			// Act
			this.RemoveMappingController.RemoveMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControl.Received().DeleteMapping(AKISBV_5_0_35);
			this.Environment.Received().ResetLocalWebserver();
			this.FileSystem.Received().DeleteDirectory(LOCAL_PATH_AKISBV_5_0_35);
			this.AdeNet.Received().CleanupIIS(AKISBV_5_0_35);
			this.Database.Received().DeleteLocalDump(AKISBV_5_0_35);
			this.Ablage.Received().Remove(AKISBV_5_0_35);
		}
		#endregion
	}
}