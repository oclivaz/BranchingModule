using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class AdeNetServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private static readonly string ADENET_EXE_PATH = SettingsDummy.AdeNetExePath;
		private static readonly string BUILDCONFIGURATION_URL = SettingsDummy.BuildConfigurationUrl;
		#endregion

		#region Properties
		private IAdeNetService AdeNetService { get; set; }

		private ISettings Settings { get; set; }

		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.Settings.AdeNetExePath.Returns(ADENET_EXE_PATH);
			this.Settings.BuildConfigurationUrl.Returns(BUILDCONFIGURATION_URL);
			this.FileExecution = Substitute.For<IFileExecutionService>();

			this.AdeNetService = new AdeNetService(this.FileExecution, new ConventionDummy(), this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestInstallPackages()
		{
			// Arrange
			ITeamProjectSettings teamprojectSettings = TeamProjectSettings("someDB", "someDB", new[] { ASKFB });
			this.Settings.GetTeamProjectSettings(AKISBV).Returns(teamprojectSettings);
			this.FileExecution.ExecuteInCmd(ADENET_EXE_PATH, "-list AskFB").Returns(@"AskFB: 3.1.0.1
AskFB: 3.1.0.1-Schubidu
");

			// Act
			this.AdeNetService.InstallPackages(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-workingdirectory {0} -deploy -development", LOCAL_PATH_AKISBV_5_0_35));
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-workingdirectory {0} -install AskFB 3.1.0.1 -development", LOCAL_PATH_AKISBV_5_0_35));
		}

		[TestMethod]
		public void TestBuildWebConfig()
		{
			// Act
			this.AdeNetService.BuildWebConfig(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-workingdirectory {0} -buildwebconfig -development", LOCAL_PATH_AKISBV_5_0_35));
		}

		[TestMethod]
		public void TestInitializeIIS()
		{
			// Act
			this.AdeNetService.InitializeIIS(AKISBV_5_0_35);

			// Asseri
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-initializeiis {0}", LOCAL_PATH_AKISBV_5_0_35));
		}

		[TestMethod]
		public void TestCleanupIIS()
		{
			// Act
			this.AdeNetService.CleanupIIS(AKISBV_5_0_35);

			// Asseri
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-cleanupiis {0}", LOCAL_PATH_AKISBV_5_0_35));
		}

		[TestMethod]
		public void TestCreateBuildDefinition()
		{
			// Act
			this.AdeNetService.CreateBuildDefinition(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.INTERNET_EXPLORER, BUILDCONFIGURATION_URL);
		}

		[TestMethod]
		public void TestCreateDatabase()
		{
			// Act
			this.AdeNetService.CreateDatabase(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(ADENET_EXE_PATH, string.Format(@"-createdb {0}", LOCAL_PATH_AKISBV_5_0_35));
		}
		#endregion
	}
}