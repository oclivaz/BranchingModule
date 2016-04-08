using System;
using System.IO;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.Builder;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class DumpRepositoryServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private const string LOCAL_DUMP = @"c:\temp\localdump.bak";
		#endregion

		#region Fields
		private readonly DateTimeBuilder MONDAY = new DateTimeBuilder(10, 08, 2015);
		private readonly DateTimeBuilder A_LONG_TIME_AGO = new DateTimeBuilder(10, 08, 2010);
		private const string ASK = "ASK";
		#endregion

		#region Properties
		private IDumpRepositoryService DumpRepository { get; set; }
		private ISettings Settings { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.Convention = new ConventionDummy();
			this.DumpRepository = new DumpRepositoryService(this.VersionControl, this.FileSystem, this.Convention, this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCopyDump_Releasebranch()
		{
			// Arrange
			this.VersionControl.GetCreationTime(AKISBV_5_0_35).Returns(MONDAY.At(09, 30));
			this.Settings.DumpRepositoryPath.Returns(@"Y:\DumpRepository");
			this.Settings.TempDirectory.Returns(@"c:\tempDir");
			this.Settings.GetTeamProjectSettings("AkisBV").Returns(TeamProjectSettings("egal", ASK));

			IFileInfo[] dumpArchives =
			{
				FileInfo(@"Y:\DumpRepository\ASK_20150810_1.zip", MONDAY.At(09, 15)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_2.zip", MONDAY.At(09, 22)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_3.zip", MONDAY.At(09, 37)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_4.zip", MONDAY.At(09, 38))
			};

			this.FileSystem.GetFiles(@"Y:\DumpRepository").Returns(dumpArchives);

			// Act
			this.DumpRepository.CopyDump(AKISBV_5_0_35, LOCAL_DUMP);

			// Assert
			this.FileSystem.Received().Copy(@"Y:\DumpRepository\ASK_20150810_2.zip", @"c:\tempDir\ASK_20150810_2.zip");
			this.FileSystem.Received().ExtractZip(@"c:\tempDir\ASK_20150810_2.zip", @"c:\tempDir");
			this.FileSystem.Received().Move(@"c:\tempDir\ASK.bak", LOCAL_DUMP);
			this.FileSystem.Received().DeleteFile(@"c:\tempDir\ASK_20150810_2.zip");
		}

		[TestMethod]
		public void TestCopyDump_Mainbranch()
		{
			// Arrange
			this.VersionControl.GetCreationTime(AKISBV_MAIN).Returns(A_LONG_TIME_AGO.At(06, 14));
			this.Settings.DumpRepositoryPath.Returns(@"Y:\DumpRepository");
			this.Settings.TempDirectory.Returns(@"c:\tempDir");
			this.Settings.GetTeamProjectSettings("AkisBV").Returns(TeamProjectSettings("egal", ASK));

			IFileInfo[] dumpArchives =
			{
				FileInfo(@"Y:\DumpRepository\ASK_20150810_1.zip", MONDAY.At(09, 15)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_2.zip", MONDAY.At(09, 22)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_3.zip", MONDAY.At(09, 37)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_4.zip", MONDAY.At(09, 38))
			};

			this.FileSystem.GetFiles(@"Y:\DumpRepository").Returns(dumpArchives);

			// Act
			this.DumpRepository.CopyDump(AKISBV_MAIN, LOCAL_DUMP);

			// Assert
			this.FileSystem.Received().Copy(@"Y:\DumpRepository\ASK_20150810_4.zip", @"c:\tempDir\ASK_20150810_4.zip");
			this.FileSystem.Received().ExtractZip(@"c:\tempDir\ASK_20150810_4.zip", @"c:\tempDir");
			this.FileSystem.Received().Move(@"c:\tempDir\ASK.bak", LOCAL_DUMP);
			this.FileSystem.Received().DeleteFile(@"c:\tempDir\ASK_20150810_4.zip");
		}

		[TestMethod]
		public void TestCopyDump_Development_Branch()
		{
			// Arrange
			this.VersionControl.GetCreationTime(AKISBV_STD_10).Returns(A_LONG_TIME_AGO.At(06, 14));
			this.Settings.DumpRepositoryPath.Returns(@"Y:\DumpRepository");
			this.Settings.TempDirectory.Returns(@"c:\tempDir");
			this.Settings.GetTeamProjectSettings("AkisBV").Returns(TeamProjectSettings("egal", ASK));

			IFileInfo[] dumpArchives =
			{
				FileInfo(@"Y:\DumpRepository\ASK_20150810_1.zip", MONDAY.At(09, 15)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_2.zip", MONDAY.At(09, 22)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_3.zip", MONDAY.At(09, 37)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_4.zip", MONDAY.At(09, 38))
			};

			this.FileSystem.GetFiles(@"Y:\DumpRepository").Returns(dumpArchives);

			// Act
			this.DumpRepository.CopyDump(AKISBV_STD_10, LOCAL_DUMP);

			// Assert
			this.FileSystem.Received().Copy(@"Y:\DumpRepository\ASK_20150810_4.zip", @"c:\tempDir\ASK_20150810_4.zip");
			this.FileSystem.Received().ExtractZip(@"c:\tempDir\ASK_20150810_4.zip", @"c:\tempDir");
			this.FileSystem.Received().Move(@"c:\tempDir\ASK.bak", LOCAL_DUMP);
			this.FileSystem.Received().DeleteFile(@"c:\tempDir\ASK_20150810_4.zip");
		}

		[TestMethod]
		public void TestCopyDump_no_Dump_before_branch_creation()
		{
			// Arrange
			this.VersionControl.GetCreationTime(AKISBV_5_0_35).Returns(MONDAY.At(09, 30));
			this.Settings.DumpRepositoryPath.Returns(@"Y:\DumpRepository");
			this.Settings.TempDirectory.Returns(@"c:\tempDir");
			this.Settings.GetTeamProjectSettings("AkisBV").Returns(TeamProjectSettings("egal", ASK));

			IFileInfo[] dumpArchives =
			{
				FileInfo(@"Y:\DumpRepository\ASK_20150810_3.zip", MONDAY.At(09, 37)),
				FileInfo(@"Y:\DumpRepository\ASK_20150810_4.zip", MONDAY.At(09, 38))
			};

			this.FileSystem.GetFiles(@"Y:\DumpRepository").Returns(dumpArchives);

			// Act
			try
			{
				this.DumpRepository.CopyDump(AKISBV_5_0_35, LOCAL_DUMP);
				Assert.Fail("Exception was Expected");
			}
			catch(Exception e)
			{
				StringAssert.Contains(e.Message, "No dump");
			}
		}
		#endregion

		#region Privates
		private IFileInfo FileInfo(string strFullName, DateTime dtCreationTime)
		{
			IFileInfo fileInfo = Substitute.For<IFileInfo>();

			fileInfo.FullName.Returns(strFullName);
			fileInfo.FileName.Returns(Path.GetFileName(strFullName));
			fileInfo.CreationTime.Returns(dtCreationTime);

			return fileInfo;
		}
		#endregion
	}
}