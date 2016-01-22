using System.Collections.Generic;
using System.Linq;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest
{
	[TestClass]
	public class SettingsTest : BranchingModuleTestBase
	{
		#region Tests
		[TestMethod]
		public void TestCreation()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();

			// Act
			ISettings settings = new Settings(dto);

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

			ITeamProjectSettings teamProjectSettings = settings.GetTeamProjectSettings(AKISBV);
			Assert.IsNotNull(teamProjectSettings);
			Assert.AreEqual("LocalDB", teamProjectSettings.LocalDB);
			Assert.AreEqual("RefDB", teamProjectSettings.RefDB);
			Assert.AreEqual(ASKFB, teamProjectSettings.AditionalReferences.Single());
		}

		[TestMethod]
		public void TestGetTeamprojectSettings_wrong_Case()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();
			ISettings settings = new Settings(dto);

			// Act
			ITeamProjectSettings teamProjectSettings = settings.GetTeamProjectSettings(AKISBV);

			// Assert
			Assert.IsNotNull(teamProjectSettings);
			Assert.AreEqual("LocalDB", teamProjectSettings.LocalDB);
			Assert.AreEqual(ASKFB, teamProjectSettings.AditionalReferences.Single());
		}

		[TestMethod]
		public void TestIsSupportedTeamproject_with_supported_teamproject()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();
			ISettings settings = new Settings(dto);

			// Act
			bool bSupported = settings.IsSupportedTeamproject(AKISBV);

			// Assert
			Assert.IsTrue(bSupported);
		}

		[TestMethod]
		public void TestIsSupportedTeamproject_with_not_supported_teamproject()
		{
			// Arrange
			SettingsDTO dto = CreateSettingsDTO();
			ISettings settings = new Settings(dto);

			// Act
			bool bSupported = settings.IsSupportedTeamproject("AdeNet");

			// Assert
			Assert.IsFalse(bSupported);
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

			dto.Teamprojects.Add(AKISBV, new TeamProjectSettingsDTO
			                             {
				                             LocalDB = "LocalDB",
				                             RefDB = "RefDB",
				                             AditionalReferences = new[] { ASKFB }
			                             });
			return dto;
		}
		#endregion
	}
}