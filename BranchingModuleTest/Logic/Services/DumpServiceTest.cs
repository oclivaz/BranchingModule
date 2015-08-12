using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class DumpServiceTest : BranchingModuleTestBase
	{
		#region Fields
		private const string RESTORE_DATABASE = "RESTORE DATABASE";
		private readonly string LOCAL_DUMP = ReleasebranchConventionDummy.GetLocalDump(AKISBV_5_0_35);
		private readonly string BUILDSERVER_DUMP = ReleasebranchConventionDummy.GetBuildserverDump(AKISBV_5_0_35);
		#endregion

		#region Properties
		private IDumpService DumpService { get; set; }

		private ISettings Settings { get; set; }

		private IFileSystemAdapter FileSystem { get; set; }

		private IDumpRepositoryService DumpRepository { get; set; }

		private ISQLServerService SQLServer { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.DumpRepository = Substitute.For<IDumpRepositoryService>();
			this.SQLServer = Substitute.For<ISQLServerService>();
			this.DumpService = new DumpService(this.DumpRepository, this.FileSystem, this.SQLServer, new ConventionDummy(), this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRestoreDump_with_Dump_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(true);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DumpService.RestoreDump(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestoreDump_with_Dump_locally()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(true);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DumpService.RestoreDump(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestoreDump_with_no_Dump_yet()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(false);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DumpService.RestoreDump(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, LOCAL_DUMP);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestInstallBuildserverDump()
		{
			// Arrange
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(false);

			// Act
			this.DumpService.InstallBuildserverDump(AKISBV_5_0_35);

			// Assert
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, BUILDSERVER_DUMP);
		}

		[TestMethod]
		public void TestInstallBuildserverDump_dump_already_on_Buildserver()
		{
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(true);

			// Act
			this.DumpService.InstallBuildserverDump(AKISBV_5_0_35);

			// Assert
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
		}
		#endregion
	}
}