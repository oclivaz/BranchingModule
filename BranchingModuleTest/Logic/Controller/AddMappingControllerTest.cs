using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class AddMappingControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private AddMappingController AddMappingController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		private IDumpService Dump { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.BuildEngine = Substitute.For<IBuildEngineService>();
			this.ConfigFileService = Substitute.For<IConfigFileService>();
			this.Dump = Substitute.For<IDumpService>();
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.AddMappingController = new AddMappingController(this.VersionControl, this.AdeNet, this.BuildEngine, this.ConfigFileService, this.Dump, this.FileExecution, new ConventionDummy(),
			                                                     new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestAddMapping()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, false, false);

			// Assert
			this.VersionControl.Received().CreateMapping(AKISBV_5_0_35);
			this.AdeNet.Received().InstallPackages(AKISBV_5_0_35);
			this.ConfigFileService.Received().CreateIndivConfig(AKISBV_5_0_35);
			this.VersionControl.Received().CreateAppConfig(AKISBV_5_0_35);
			this.AdeNet.Received().BuildWebConfig(AKISBV_5_0_35);
			this.BuildEngine.Received().Build(AKISBV_5_0_35);
			this.AdeNet.Received().InitializeIIS(AKISBV_5_0_35);
			this.Dump.Received().RestoreDump(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestAddMapping_minimal()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, true, false);

			// Assert
			this.VersionControl.Received().CreateMapping(AKISBV_5_0_35);
			this.AdeNet.DidNotReceive().InstallPackages(Arg.Any<BranchInfo>());
			this.ConfigFileService.DidNotReceive().CreateIndivConfig(Arg.Any<BranchInfo>());
			this.VersionControl.DidNotReceive().CreateAppConfig(Arg.Any<BranchInfo>());
			this.AdeNet.DidNotReceive().BuildWebConfig(Arg.Any<BranchInfo>());
			this.BuildEngine.DidNotReceive().Build(Arg.Any<BranchInfo>());
			this.AdeNet.DidNotReceive().InitializeIIS(Arg.Any<BranchInfo>());
			this.Dump.DidNotReceive().RestoreDump(Arg.Any<BranchInfo>());
		}

		[TestMethod]
		public void TestAddMapping_minimal_and_opensolution()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, true, true);

			// Assert
			this.VersionControl.Received().CreateMapping(AKISBV_5_0_35);
			this.AdeNet.DidNotReceive().InstallPackages(Arg.Any<BranchInfo>());
			this.ConfigFileService.DidNotReceive().CreateIndivConfig(Arg.Any<BranchInfo>());
			this.VersionControl.DidNotReceive().CreateAppConfig(Arg.Any<BranchInfo>());
			this.AdeNet.DidNotReceive().BuildWebConfig(Arg.Any<BranchInfo>());
			this.BuildEngine.DidNotReceive().Build(Arg.Any<BranchInfo>());
			this.AdeNet.DidNotReceive().InitializeIIS(Arg.Any<BranchInfo>());
			this.Dump.DidNotReceive().RestoreDump(Arg.Any<BranchInfo>());
			this.FileExecution.Received().StartProcess(Executables.EXPLORER, LOCAL_SOLUTION_FILE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestAddMapping_openSolution()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, false, true);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.EXPLORER, LOCAL_SOLUTION_FILE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestAddMapping_dont_openSolution()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, false, false);

			// Assert
			this.FileExecution.DidNotReceive().StartProcess(Executables.EXPLORER, LOCAL_PATH_AKISBV_5_0_35);
		}
		#endregion
	}
}