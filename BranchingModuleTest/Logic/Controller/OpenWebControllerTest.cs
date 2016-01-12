using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class OpenWebControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private OpenWebController OpenWebController { get; set; }
		private IEnvironmentService Environment { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Environment = Substitute.For<IEnvironmentService>();
			this.OpenWebController = new OpenWebController(this.Environment);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.OpenWebController.OpenWeb(AKISBV_5_0_35);

			// Assert
			this.Environment.Received().OpenWeb(AKISBV_5_0_35);
		}
		#endregion
	}
}