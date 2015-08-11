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
		private readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBV", "5.0.35");
		private readonly DateTimeBuilder MONDAY = new DateTimeBuilder(10, 08, 2015);
		private const string ASK = "ASK";
		#endregion

		#region Properties
		private IDumpRepositoryService DumpRepository { get; set; }

		private ISettings Settings { get; set; }

		private IFileSystemAdapter FileSystem { get; set; }

		private IVersionControlService VersionControl { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Settings = Substitute.For<ISettings>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.DumpRepository = new DumpRepositoryService(this.VersionControl, this.FileSystem, this.Settings, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCopyDump()
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