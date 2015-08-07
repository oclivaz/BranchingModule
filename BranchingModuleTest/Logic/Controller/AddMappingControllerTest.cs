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
		private ISettings Settings { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.AddMappingController = new AddMappingController(this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestProcess()
		{
			// Arrange
			this.Settings.TeamFoundationServerPath.Returns("schubidu");

			// Act
			this.AddMappingController.Process("AkisBVBL", "1.2.3");
		}
		#endregion
	}
}