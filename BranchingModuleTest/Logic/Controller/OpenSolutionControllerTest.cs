using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class OpenSolutionControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private OpenSolutionController OpenSolutionController { get; set; }
		private IEnvironmentService Environment { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Environment = Substitute.For<IEnvironmentService>();
			this.OpenSolutionController = new OpenSolutionController(this.Environment);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.OpenSolutionController.OpenSolution(AKISBV_5_0_35);

			// Assert
			this.Environment.Received().OpenSolution(AKISBV_5_0_35);
		}
		#endregion
	}
}