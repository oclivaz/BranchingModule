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
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.SourceControl = Substitute.For<ISourceControlAdapter>();
			this.AdeNet = Substitute.For<IAdeNetAdapter>();
			this.AddMappingController = new AddMappingController(this.SourceControl, this.AdeNet);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestProcess()
		{
			// Act
			this.AddMappingController.Process("AkisBVBL", "1.2.3");

			// Assert
			this.SourceControl.Received().CreateMapping("AkisBVBL", "1.2.3");
			this.AdeNet.Received().InstallPackages("AkisBVBL", "1.2.3");
		}
		#endregion
	}
}