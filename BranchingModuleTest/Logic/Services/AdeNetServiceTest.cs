using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class AdeNetServiceTest
	{
		#region Fields
		private readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		private const string BUILDCONFIGURATION_URL = "http://www.probierä.ch";
		#endregion

		#region Properties
		private IAdeNetService AdeNetService { get; set; }

		private ISettings Settings { get; set; }

		private IConvention Convention { get; set; }

		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.FileExecution = Substitute.For<IFileExecutionService>();

			this.AdeNetService = new AdeNetService(this.FileExecution, this.Convention, this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestInstallPackages()
		{
			// Arrange
			this.Settings.AdeNetExePath.Returns(@"c:\PathToAdeNet");
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(@"c:\Solution");

			// Act
			this.AdeNetService.InstallPackages(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -deploy -development");
		}

		[TestMethod]
		public void TestBuildWebConfig()
		{
			// Arrange
			this.Settings.AdeNetExePath.Returns(@"c:\PathToAdeNet");
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(@"c:\Solution");

			// Act
			this.AdeNetService.BuildWebConfig(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -buildwebconfig -development");
		}

		[TestMethod]
		public void TestInitializeIIS()
		{
			// Arrange
			this.Settings.AdeNetExePath.Returns(@"c:\PathToAdeNet");
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(@"c:\Solution");

			// Act
			this.AdeNetService.InitializeIIS(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -initializeiis -development");
		}

		[TestMethod]
		public void TestCreateBuildDefinition()
		{
			// Arrange
			this.Settings.BuildConfigurationUrl.Returns(BUILDCONFIGURATION_URL);
			
			// Act
			this.AdeNetService.CreateBuildDefinition(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.INTERNET_EXPLORER, BUILDCONFIGURATION_URL);
		}
		#endregion
	}
}