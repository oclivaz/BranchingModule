using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core.Arguments;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class DumpServiceTest
	{
		#region Fields
		private readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		private const string RESTORE_DATABASE = "RESTORE DATABASE";
		private const string LOCAL_DUMP = @"c:\dump.bak";
		private const string BUILDSERVER_DUMP = @"\\buildserver\dump.bak";
		#endregion

		#region Properties
		private IDumpService DumpService { get; set; }

		private ISettings Settings { get; set; }

		private IConvention Convention { get; set; }

		private IFileSystemService FileSystem { get; set; }

		private IDumpRepositoryService DumpRepository { get; set; }

		private ISQLServerService SQLServer { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemService>();
			this.DumpRepository = Substitute.For<IDumpRepositoryService>();
			this.SQLServer = Substitute.For<ISQLServerService>();
			this.DumpService = new DumpService(this.DumpRepository, this.FileSystem, this.SQLServer, this.Convention, this.Settings);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRestoreDump_with_Dump_on_Buildserver()
		{
			// Arrange
			this.Convention.GetBuildserverDump(AKISBV_5_0_35).Returns(BUILDSERVER_DUMP);
			this.Convention.GetLocalDump(AKISBV_5_0_35).Returns(LOCAL_DUMP);
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
			this.Convention.GetBuildserverDump(AKISBV_5_0_35).Returns(BUILDSERVER_DUMP);
			this.Convention.GetLocalDump(AKISBV_5_0_35).Returns(LOCAL_DUMP);
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
			this.Convention.GetBuildserverDump(AKISBV_5_0_35).Returns(BUILDSERVER_DUMP);
			this.Convention.GetLocalDump(AKISBV_5_0_35).Returns(LOCAL_DUMP);
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
		#endregion
	}
}