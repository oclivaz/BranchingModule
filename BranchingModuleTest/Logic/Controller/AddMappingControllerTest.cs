using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class AddMappingControllerTest
	{
		#region Properties
		private AddMappingController AddMappingController { get; set; }
		private ISourceControlService SourceControl { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		private IDumpService Dump { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.SourceControl = Substitute.For<ISourceControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.BuildEngine = Substitute.For<IBuildEngineService>();
			this.ConfigFileService = Substitute.For<IConfigFileService>();
			this.Dump = Substitute.For<IDumpService>();
			this.AddMappingController = new AddMappingController(this.SourceControl, this.AdeNet, this.BuildEngine, this.ConfigFileService, this.Dump, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestProcess()
		{
			// Arrange
			BranchInfo branch = new BranchInfo("AkisBVBL", "1.2.3");

			// Act
			this.AddMappingController.Process(branch);

			// Assert
			this.SourceControl.Received().CreateMapping(branch);
			this.AdeNet.Received().InstallPackages(branch);
			this.ConfigFileService.Received().CreateIndivConfig(branch);
			this.SourceControl.Received().CreateAppConfig(branch);
			this.AdeNet.Received().BuildWebConfig(branch);
			this.BuildEngine.Received().Build(branch);
			this.AdeNet.Received().InitializeIIS(branch);
			this.Dump.Received().RestoreDump(branch);
		}
		#endregion
	}
}