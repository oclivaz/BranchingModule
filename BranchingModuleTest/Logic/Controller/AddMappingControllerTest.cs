using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class AddMappingControllerTest
	{
		#region Properties
		private AddMappingController AddMappingController { get; set; }
		private ISourceControlAdapter SourceControl { get; set; }
		private IAdeNetAdapter AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.SourceControl = Substitute.For<ISourceControlAdapter>();
			this.AdeNet = Substitute.For<IAdeNetAdapter>();
			this.ConfigFileService = Substitute.For<IConfigFileService>();
			this.AddMappingController = new AddMappingController(this.SourceControl, this.AdeNet, this.ConfigFileService);
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
		}
		#endregion
	}
}