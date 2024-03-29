﻿using System.Text;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class ConfigFileServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private const string AKISBV_APPCONFIG_PATH = "AkisBV app.config Path";

		private const string INDIV_CONFIG_AKISBV_5_0_35 = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <appSettings>
        <add key=""ConnectionString"" value=""server=localhost;Database=AskNet;User ID=sa;Pwd=password-123"" />
        <add key=""SMTPServer"" value=""193.135.175.18"" /> 
        <add key=""AppTitle"" value=""AkisBV 5.0.35"" />
        <add key=""Company"" value=""M-S¦Pension"" />
        <add key=""EnableUserThemes"" value=""true"" />
        <add key=""SessionKeepAlive"" value=""true"" />
    </appSettings>";
		#endregion

		#region Properties
		private IConfigFileService ConfigFileService { get; set; }
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
			this.ConfigFileService = new ConfigFileService(new ConventionDummy(), this.Settings, this.FileSystem);
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCreateIndivConfig()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV).Returns(TeamProjectSettings("AskNet", "egal"));

			// Act
			this.ConfigFileService.CreateIndivConfig(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().WriteAllText(string.Format(@"{0}\Web\Indiv\indiv.config", LOCAL_PATH_AKISBV_5_0_35), INDIV_CONFIG_AKISBV_5_0_35, Encoding.UTF8);
		}

		[TestMethod]
		public void TestCreateAppConfig()
		{
			// Arrange
			this.Settings.GetTeamProjectSettings(AKISBV).Returns(TeamProjectSettings("AskNet", "egal", AKISBV_APPCONFIG_PATH));

			// Act
			this.ConfigFileService.CreateAppConfig(AKISBV_5_0_35);

			// Assert
			this.FileSystem.Received().Copy(AKISBV_APPCONFIG_PATH, string.Format(@"{0}\Web\app.config", LOCAL_PATH_AKISBV_5_0_35));
		}
		#endregion
	}
}