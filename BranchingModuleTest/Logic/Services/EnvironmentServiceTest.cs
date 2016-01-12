using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class EnvironmentServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private static readonly string APPLICATION_NAME_AKISBV_5_0_35 = ReleasebranchConventionDummy.GetApplicationName(AKISBV_5_0_35);
		#endregion

		#region Properties
		private IEnvironmentService Environment { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.FileExecution = Substitute.For<IFileExecutionService>();

			this.Environment = new EnvironmentService(this.FileExecution, new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestOpenSolution()
		{
			// Act
			this.Environment.OpenSolution(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.EXPLORER, ReleasebranchConventionDummy.GetSolutionFile(AKISBV_5_0_35));
		}

		[TestMethod]
		public void TestOpenWeb()
		{
			// Act
			this.Environment.OpenWeb(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().StartProcess(Executables.INTERNET_EXPLORER, Arg.Is<string>(s => s.Contains(APPLICATION_NAME_AKISBV_5_0_35)));
		}
		#endregion
	}
}