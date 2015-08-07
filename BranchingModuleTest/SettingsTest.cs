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
			SettingsDTO dto = new SettingsDTO
			                  {
				                  TfsPath = "TfsPath",
				                  MSBuildPath = "MSBuildPath",
				                  TFPath = "TFPath",
				                  AdeNetPath = "AdeNetPath",
				                  BuildConfigurationUrl = "BuildConfigurationUrl",
				                  DumpRepositoryPath = "DumpRepositoryPath",
				                  LogfilePath = "LogfilePath",
				                  Teamprojects = new Dictionary<string, TeamProjectSettingsDTO>()
			                  };

			dto.Teamprojects.Add("a_teamproject", new TeamProjectSettingsDTO
			                                      {
				                                      LocalDB = "LocalDB",
				                                      RefDB = "RefDB"
			                                      });

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

			ITeamProjectSettings teamProjectSettings = settings.GetTeamProjectSettings("a_teamproject");
			Assert.IsNotNull(teamProjectSettings);
			Assert.AreEqual("LocalDB", teamProjectSettings.LocalDB);
			Assert.AreEqual("RefDB", teamProjectSettings.RefDB);
		}
		#endregion
	}
}