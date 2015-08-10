using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class MsBuildServiceTest
	{
		#region Fields
		private readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		#endregion

		#region Properties
		private IBuildEngineService MsBuildService { get; set; }

		private ISettings Settings { get; set; }

		private IConvention Convention { get; set; }

		private IFileExecutionService FileExecution { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.FileExecution = Substitute.For<IFileExecutionService>();
			this.MsBuildService = new MsBuildService(this.FileExecution, this.Convention, this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestBuild()
		{
			// Arrange
			this.Settings.MSBuildExePath.Returns(@"c:\PathToMSBuild\MSBuild.exe");
			this.Convention.GetLocalPath(AKISBV_5_0_35).Returns(@"c:\Solution");

			// Act
			this.MsBuildService.Build(AKISBV_5_0_35);

			// Assert
			this.FileExecution.Received().Execute(@"c:\PathToMSBuild\MSBuild.exe", @"c:\Solution\AkisBV.sln /t:Build");
		}
		#endregion
	}
}