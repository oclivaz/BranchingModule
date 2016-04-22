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
		private readonly DateTime CHRISTMAS_2014 = new DateTime(2014, 12, 25, 13, 25, 01);
		private readonly DateTime EASTER_2015 = new DateTime(2015, 04, 05, 08, 13, 02);
		#endregion

		#region Properties
		private IDatabaseService DatabaseService { get; set; }
		private ISettings Settings { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IDumpRepositoryService DumpRepository { get; set; }
		private ISQLServerAdapter SQLServer { get; set; }
		private IAdeNetService AdeNet { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.DumpRepository = Substitute.For<IDumpRepositoryService>();
			this.SQLServer = Substitute.For<ISQLServerAdapter>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.DatabaseService = new DatabaseService(this.DumpRepository, this.FileSystem, this.SQLServer, this.AdeNet, new ConventionDummy(), this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCreateDatabase()
		{
			// Arrange
			this.FileSystem.ReadAllText(Arg.Is<string>(script => script.Contains("Create"))).Returns("Create");

			// Act
			this.DatabaseService.Create(AKISBV_5_0_35);

			// Assert
			this.AdeNet.Received().CreateDatabase(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_Dump_only_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(true);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_Dump_only_locally()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(false);
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_newer_dump_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);
			IFileInfo dumpModifiedOnChristmas2014 = DumpModifiedOn(CHRISTMAS_2014);
			this.FileSystem.GetFileInfo(LOCAL_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnChristmas2014);

			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(true);
			IFileInfo dumpModifiedOnEaster2015 = DumpModifiedOn(EASTER_2015);
			this.FileSystem.GetFileInfo(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnEaster2015);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_same_Dump_locally()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);
			IFileInfo dumpModifiedOnChristmas2014 = DumpModifiedOn(CHRISTMAS_2014);
			this.FileSystem.GetFileInfo(LOCAL_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnChristmas2014);

			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(true);
			this.FileSystem.GetFileInfo(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnChristmas2014);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_older_Dump_on_buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);
			IFileInfo dumpModifiedOnEaster2015 = DumpModifiedOn(EASTER_2015);
			this.FileSystem.GetFileInfo(LOCAL_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnEaster2015);

			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(true);

			IFileInfo dumpModifiedOnChristmas2014 = DumpModifiedOn(CHRISTMAS_2014);
			this.FileSystem.GetFileInfo(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(dumpModifiedOnChristmas2014);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.DidNotReceive().CopyDump(Arg.Any<BranchInfo>(), Arg.Any<string>());
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Releasebranch_with_no_Dump_yet()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(false);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, LOCAL_DUMP_AKISBV_5_0_35);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Mainbranch()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_MAIN).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_MAIN).Returns(true);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_MAIN);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_MAIN, LOCAL_DUMP_AKISBV_MAIN);
			this.DumpRepository.Received().CopyDump(AKISBV_MAIN, LOCAL_DUMP_AKISBV_MAIN);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Development_Branch_Dump_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_STD_10).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_STD_10).Returns(true);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_STD_10);

			// Assert
			this.FileSystem.Received().Copy(BUILDSERVER_DUMP_AKISBV_STD_10, LOCAL_DUMP_AKISBV_STD_10);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_Development_Branch_no_Dump_on_Buildserver()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_STD_10).Returns(false);
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_STD_10).Returns(false);

			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns(RESTORE_DATABASE);

			// Act
			this.DatabaseService.Restore(AKISBV_STD_10);

			// Assert
			this.FileSystem.DidNotReceive().Copy(BUILDSERVER_DUMP_AKISBV_STD_10, LOCAL_DUMP_AKISBV_STD_10);
			this.DumpRepository.Received().CopyDump(AKISBV_STD_10, LOCAL_DUMP_AKISBV_STD_10);
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(script => script.Equals(RESTORE_DATABASE)), Arg.Any<string>());
		}

		[TestMethod]
		public void TestRestore_with_exception_while_restoring()
		{
			// Arrange
			this.FileSystem.Exists(LOCAL_DUMP_AKISBV_5_0_35).Returns(true);
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
		public void TestRestore_with_explicit_file()
		{
			// Arrange
			this.FileSystem.ReadAllText(Arg.Is<string>(filename => filename.Contains("Restore"))).Returns("Restore {Dump}");

			// Act
			this.DatabaseService.Restore(AKISBV_5_0_35, "my_special_file");

			// Assert
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(s => s.Contains("Restore") && s.Contains("my_special_file")), Arg.Any<string>());
		}

		[TestMethod]
		public void TestInstallBuildserverDump()
		{
			// Arrange
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(false);

			// Act
			this.DatabaseService.InstallBuildserverDump(AKISBV_5_0_35);

			// Assert
			this.DumpRepository.Received().CopyDump(AKISBV_5_0_35, BUILDSERVER_DUMP_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestInstallBuildserverDump_dump_already_on_Buildserver()
		{
			this.FileSystem.Exists(BUILDSERVER_DUMP_AKISBV_5_0_35).Returns(true);

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

		[TestMethod]
		public void TestBackupDatabase_with_filepath()
		{
			// Arrange
			this.FileSystem.ReadAllText(Arg.Is<string>(script => script.Contains("Backup"))).Returns("Backup {Dump}");

			// Act
			this.DatabaseService.Backup(AKISBV_5_0_35, "my_special_bak_file");

			// Assert
			this.SQLServer.Received().ExecuteScript(Arg.Is<string>(s => s.Contains("my_special_bak_file") && s.Contains("Backup")), Arg.Any<string>());
		}
		#endregion

		#region Privates
		private IFileInfo DumpModifiedOn(DateTime dtModification)
		{
			IFileInfo localDumpInfo = Substitute.For<IFileInfo>();
			localDumpInfo.ModificationTime.Returns(dtModification);
			return localDumpInfo;
		}

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