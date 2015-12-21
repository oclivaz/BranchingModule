﻿using BranchingModule.Logic;
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
		private IFileExecutionService FileExecution { get; set; }
		private IAblageService Ablage { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.BuildEngine = Substitute.For<IBuildEngineService>();
			this.Database = Substitute.For<IDatabaseService>();
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.Ablage = Substitute.For<IAblageService>();
			this.GetLatestController = new GetLatestController(this.VersionControl, this.AdeNet, this.BuildEngine, this.Database, this.FileExecution, this.Ablage, new ConventionDummy(),
			                                                   new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLatest()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false);

			// Assert
			this.VersionControl.Received().GetLatest(AKISBV_5_0_35);
			this.AdeNet.Received().InstallPackages(AKISBV_5_0_35);
			this.BuildEngine.Received().Build(AKISBV_5_0_35);
			this.Database.Received().Restore(AKISBV_5_0_35);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_openSolution()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, true);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.EXPLORER, LOCAL_SOLUTION_FILE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestGetLatest_dont_openSolution()
		{
			// Act
			this.GetLatestController.GetLatest(AKISBV_5_0_35, false);

			// Assert
			this.FileExecution.DidNotReceive().StartProcess(Executables.EXPLORER, LOCAL_PATH_AKISBV_5_0_35);
		}
		#endregion
	}
}