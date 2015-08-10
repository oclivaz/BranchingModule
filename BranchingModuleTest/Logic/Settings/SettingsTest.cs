using System.Collections.Generic;
using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest
{
	[TestClass]
	public class SettingsTest
	{
		#region Tests
		[TestMethod]
		public void TestCreation()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();

			// Act
			Settings settings = new Settings(dto);

			// Assert
			Assert.AreEqual(dto.TfsPath, settings.TeamFoundationServerPath);
			Assert.AreEqual(dto.MSBuildPath, settings.MSBuildExePath);
			Assert.AreEqual(dto.TFPath, settings.TFExePath);
			Assert.AreEqual(dto.AdeNetPath, settings.AdeNetExePath);
			Assert.AreEqual(dto.BuildConfigurationUrl, settings.BuildConfigurationUrl);
			Assert.AreEqual(dto.DumpRepositoryPath, settings.DumpRepositoryPath);
			Assert.AreEqual(dto.LogfilePath, settings.LogfilePath);
			Assert.AreEqual(dto.AppConfigServerPath, settings.AppConfigServerPath);
			Assert.AreEqual(dto.TempDirectory, settings.TempDirectory);
			Assert.AreEqual(dto.SQLScriptPath, settings.SQLScriptPath);
			Assert.AreEqual(dto.SQLConnectionString, settings.SQLConnectionString);

			ITeamProjectSettings teamProjectSettings = settings.GetTeamProjectSettings("a_teamproject");
			Assert.IsNotNull(teamProjectSettings);
			Assert.AreEqual("LocalDB", teamProjectSettings.LocalDB);
			Assert.AreEqual("RefDB", teamProjectSettings.RefDB);
		}

		[TestMethod]
		public void TestGetTeamprojectSettings_wrong_Case()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();
			Settings settings = new Settings(dto);

			// Act
			ITeamProjectSettings teamProjectSettings = settings.GetTeamProjectSettings("A_TEAMPROJECT");

			// Assert
			Assert.IsNotNull(teamProjectSettings);
			Assert.AreEqual("LocalDB", teamProjectSettings.LocalDB);
			Assert.AreEqual("RefDB", teamProjectSettings.RefDB);
		}
		#endregion

		#region Privates
		private static SettingsDTO CreateSettingsDTO()
		{
			SettingsDTO dto = new SettingsDTO
			                  {
				                  TfsPath = "TfsPath",
				                  MSBuildPath = "MSBuildPath",
				                  TFPath = "TFPath",
				                  AdeNetPath = "AdeNetPath",
				                  BuildConfigurationUrl = "BuildConfigurationUrl",
				                  DumpRepositoryPath = "DumpRepositoryPath",
				                  LogfilePath = "LogfilePath",
				                  AppConfigServerPath = "AppConfigServerPath",
				                  TempDirectory = "TempDirectory",
				                  SQLScriptPath = "SQLScriptPath",
				                  SQLConnectionString = "SQLConnectionString",
				                  Teamprojects = new Dictionary<string, TeamProjectSettingsDTO>()
			                  };

			dto.Teamprojects.Add("a_teamproject", new TeamProjectSettingsDTO
			                                      {
				                                      LocalDB = "LocalDB",
				                                      RefDB = "RefDB"
			                                      });
			return dto;
		}
		#endregion
	}
}