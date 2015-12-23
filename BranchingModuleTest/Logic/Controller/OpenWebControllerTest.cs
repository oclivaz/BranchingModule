using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class OpenWebControllerTest : BranchingModuleTestBase
	{
		#region Constants
		private static readonly string APPLICATION_NAME_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetApplicationName(AKISBV_5_0_35);
		#endregion

		#region Properties
		private OpenWebController OpenWebController { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.OpenWebController = new OpenWebController(this.FileExecution, new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.OpenWebController.OpenWeb(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.INTERNET_EXPLORER, Arg.Is<string>(s => s.Contains(APPLICATION_NAME_AKISBV_5_0_35)));
		}
		#endregion
	}
}