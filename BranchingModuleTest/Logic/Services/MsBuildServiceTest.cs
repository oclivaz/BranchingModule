using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class MsBuildServiceTest : BranchingModuleTestBase
	{
		#region Properties
		private IBuildEngineService MsBuildService { get; set; }

		private ISettings Settings { get; set; }

		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.MsBuildService = new MsBuildService(this.FileExecution, new ConventionDummy(), this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestBuild()
		{
			// Arrange
			this.Settings.MSBuildExePath.Returns(@"c:\PathToMSBuild\MSBuild.exe");

			// Act
			this.MsBuildService.Build(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().Execute(@"c:\PathToMSBuild\MSBuild.exe", string.Format(@"{0}\AkisBV.sln /t:Build", LOCAL_PATH_AKISBV_5_0_35));
		}
		#endregion
	}
}