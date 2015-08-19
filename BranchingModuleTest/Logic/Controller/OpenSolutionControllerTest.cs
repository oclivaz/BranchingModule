using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class OpenSolutionControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private OpenSolutionController OpenSolutionController { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.OpenSolutionController = new OpenSolutionController(this.FileExecution, new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.OpenSolutionController.OpenSolution(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.EXPLORER, LOCAL_SOLUTION_FILE_PATH_AKISBV_5_0_35);
		}
		#endregion
	}
}