using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class AdeNetServiceTest
	{
		#region Fields
		private readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		#endregion

		#region Properties
		private IAdeNetService AdeNetService { get; set; }

		private ISettings Settings { get; set; }

		private IConvention Convention { get; set; }

		private IFileSystemService FileSystem { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemService>();
			this.AdeNetService = new AdeNetService(this.FileSystem, this.Convention, this.Settings);
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
			this.FileSystem.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -deploy -development");
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
			this.FileSystem.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -buildwebconfig -development");
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
			this.FileSystem.Received().ExecuteInCmd(@"c:\PathToAdeNet\AdeNet.exe", @"-workingdirectory c:\Solution -initializeiis -development");
		}
		#endregion
	}
}