using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class DatabaseServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private const string RESTORE_DATABASE = "RESTORE DATABASE";
		#endregion

		#region Fields
		private readonly string LOCAL_DUMP = ReleasebranchConventionDummy.GetLocalDump(AKISBV_5_0_35);
		private readonly string BUILDSERVER_DUMP = ReleasebranchConventionDummy.GetBuildserverDump(AKISBV_5_0_35);
		#endregion

		#region Properties
		private IDatabaseService DatabaseService { get; set; }

		private ISettings Settings { get; set; }

		private IFileSystemAdapter FileSystem { get; set; }

		private IDumpRepositoryService DumpRepository { get; set; }

		private ISQLServerAdapter SQLServer { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.DumpRepository = Substitute.For<IDumpRepositoryService>();
			this.SQLServer = Substitute.For<ISQLServerAdapter>();
			this.DatabaseService = new DatabaseService(this.DumpRepository, this.FileSystem, this.SQLServer, new ConventionDummy(), this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRestore_with_Dump_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(true);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_with_Dump_locally()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(true);
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(false);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.Received().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_with_no_Dump_yet()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(false);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP, LOCAL_DUMP);
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, LOCAL_DUMP);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_with_exception_while_restoring()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP).Returns(true);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Kill"))).Returns("Kill");
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Post"))).Returns("Post");

			RestoreDatabaseThrowsExceptionForFirstTwoTimes();

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.SQLServer.Received(3).ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestInstallBuildserverDump()
		{
			// Arrange
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(false);

			// Act
			this.DatabaseService.InstallBuildserverDump(AKISBV_5_0_35);

			// Assert
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, BUILDSERVER_DUMP);
		}

		[TestMethod]
		public void TestInstallBuildserverDump_dump_already_on_Buildserver()
		{
			this.FileSystem.Exists(BUILDSERVER_DUMP).Returns(true);

			// Act
			this.DatabaseService.InstallBuildserverDump(AKISBV_5_0_35);

			// Assert
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
		}

		[TestMethod]
		public void TestDeleteLocalDump_with_local_dump()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);

			// Act
			this.DatabaseService.DeleteLocalDump(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().DeleteFile(LOCAL_DUMP_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestDeleteLocalDump_without_local_dump()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(false);
			
			// Act
			this.DatabaseService.DeleteLocalDump(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().DeleteFile(LOCAL_DUMP_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestBackupDatabase()
		{
			// Arrange
			this.FileSystem.ReadAllText(Arg.Is<string>(script => script.Contains("Backup"))).Returns("Backup");

			// Act
			this.DatabaseService.Backup(AKISBV_5_0_35);

			// Assert
			this.SQLServer.Received().ExecuteScript("Backup", Arg.Any<string>());
		}
		#endregion

		#region Privates
		private void RestoreDatabaseThrowsExceptionForFirstTwoTimes()
		{
			int i = 0;

			this.SQLServer.When(x => x.ExecuteScript(RESTORE_DATABASE, Arg.Any<string>()))
			    .Do(x =>
			        {
				        if(i < 2)
				        {
					        i++;
					        throw new Exception();
				        }
			        });
		}
		#endregion
	}
}