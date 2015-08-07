using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class AddMappingControllerTest
	{
		#region Properties
		internal AddMappingController AddMappingController { get; set; }
		private ISourceControlAdapter SourceControl { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.SourceControl = Substitute.For<ISourceControlAdapter>();
			this.AddMappingController = new AddMappingController(this.SourceControl, this.Settings);
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
		}
		#endregion
	}
}