using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class GetLatestControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private GetLatestController GetLatestController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IAdeNetService AdeNet { get; set; }
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
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();
			this.Environment = Substitute.For<IEnvironmentService>();
			this.GetLatestController = new GetLatestController(this.VersionControl, this.AdeNet, this.BuildEngine, this.Ablage, this.Environment, new ConventionDummy(),
			                                                   new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLatest()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false , false);

			// Assert
			this.VersionControl.Received().GetLatest(AKISBV_5_0_35);
			this.AdeNet.Received().InstallPackages(AKISBV_5_0_35);
			this.BuildEngine.Received().Build(AKISBV_5_0_35);
			this.Database.DidNotReceive().Restore(AKISBV_5_0_35);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_openSolution()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, true, false);

			// Assert
			this.Environment.Received().OpenSolution(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_dont_openSolution()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false, false);

			// Assert
			this.Environment.DidNotReceive().OpenSolution(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_openWeb()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false, true);

			// Assert
			this.Environment.Received().OpenWeb(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_dont_openWeb()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false, false);

			// Assert
			this.Environment.DidNotReceive().OpenWeb(AKISBV_5_0_35);
		}
		#endregion
	}
}