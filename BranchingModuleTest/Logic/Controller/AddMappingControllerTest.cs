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
			this.BuildEngine = Substitute.For<IBuildEngineService>();
			this.ConfigFileService = Substitute.For<IConfigFileService>();
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();
			this.Environment = Substitute.For<IEnvironmentService>();
			this.AddMappingController = new AddMappingController(this.VersionControl, this.AdeNet, this.BuildEngine, this.ConfigFileService, this.Database, this.Ablage, this.Environment, new ConventionDummy(),
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
			this.Database.Received().Restore(AKISBV_5_0_35);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
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
			this.Database.DidNotReceive().Restore(Arg.Any<BranchInfo>());
			this.Ablage.DidNotReceive().Reset(Arg.Any<BranchInfo>());
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
			this.Database.DidNotReceive().Restore(Arg.Any<BranchInfo>());
			this.Environment.Received().OpenSolution(AKISBV_5_0_35);
			this.Ablage.DidNotReceive().Reset(Arg.Any<BranchInfo>());
		}

		[TestMethod]
		public void TestAddMapping_openSolution()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, false, true);

			// Assert
			this.Environment.Received().OpenSolution(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestAddMapping_dont_openSolution()
		{
			// Act
			this.AddMappingController.AddMapping(AKISBV_5_0_35, false, false);

			// Assert
			this.Environment.DidNotReceive().OpenSolution(AKISBV_5_0_35);
		}
		#endregion
	}
}