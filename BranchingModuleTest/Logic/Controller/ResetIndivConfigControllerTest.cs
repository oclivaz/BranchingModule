using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class ResetIndivConfigControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private ResetIndivConfigController ResetIndivConfigController { get; set; }
		private IConfigFileService ConfigFile { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.ConfigFile = Substitute.For<IConfigFileService>();
			this.ResetIndivConfigController = new ResetIndivConfigController(this.ConfigFile);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.ResetIndivConfigController.ResetIndivConfig(AKISBV_5_0_35);

			// Assert
			this.ConfigFile.Received().CreateIndivConfig(AKISBV_5_0_35);
		}
		#endregion
	}
}