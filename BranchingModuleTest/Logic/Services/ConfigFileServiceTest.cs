using System.Text;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core.Arguments;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class ConfigFileServiceTest : BranchingModuleTestBase
	{
		#region Properties
		private IConfigFileService ConfigFileService { get; set; }
		private IConvention Convention { get; set; }
		private ISourceControlAdapter SourceControl { get; set; }
		private ISettings Settings { get; set; }
		private IFileWriter FileWriter { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Convention = Substitute.For<IConvention>();
			this.Settings = Substitute.For<ISettings>();
			this.SourceControl = Substitute.For<ISourceControlAdapter>();
			this.FileWriter = Substitute.For<IFileWriter>();
			this.ConfigFileService = new ConfigFileService(this.Convention, this.SourceControl, this.Settings, this.FileWriter);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCreateIndivConfig()
		{
			// Arrange
			this.Convention.GetLocalPath(Arg.Any<BranchInfo>()).Returns(@"c:\somewhere");
			this.Settings.GetTeamProjectSettings("AkisBVPK").Returns(TeamProjectSettings("AskNet", "egal"));

			// Act
			this.ConfigFileService.CreateIndivConfig(new BranchInfo("AkisBVPK", "5.0.34"));

			// Assert
			this.FileWriter.Received().Write(@"c:\somewhere\Web\Indiv\indiv.config", CreateSampleIndivConfig(), Encoding.UTF8);
		}

		[TestMethod]
		public void TestCreateAppConfig()
		{
			// Arrange
			this.Settings.AppConfigServerPath.Returns("$/somwhereOnTheTFS/app.config");

			BranchInfo branch = new BranchInfo("AkisBVPK", "5.0.34");
			this.Convention.GetLocalPath(branch).Returns(@"c:\somwhereLocally");

			// Act
			this.ConfigFileService.CreateAppConfig(branch);

			// Assert
			this.SourceControl.Received().DownloadFile("$/somwhereOnTheTFS/app.config", @"c:\somwhereLocally\Web\app.config");
		}
		#endregion

		#region Privates
		private static string CreateSampleIndivConfig()
		{
			return @"<?xml version=""1.0"" encoding=""utf-8""?>
    <appSettings>
        <add key=""ConnectionString"" value=""server=localhost;Database=AskNet;User ID=sa;Pwd=password-123"" />
        <add key=""SMTPServer"" value=""193.135.175.18"" /> 
        <add key=""AppTitle"" value=""AkisBVPK Release 5.0.34"" />
        <add key=""Company"" value=""M-S¦Pension"" />
        <add key=""EnableUserThemes"" value=""true"" />
        <add key=""SessionKeepAlive"" value=""true"" />
    </appSettings>";
		}
		#endregion
	}
}