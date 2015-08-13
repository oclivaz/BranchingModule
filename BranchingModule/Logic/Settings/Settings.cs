using System;
using System.Collections.Generic;

namespace BranchingModule.Logic
{
	internal class Settings : ISettings
	{
		#region Constants
		public static readonly string DEFAULT_SETTINGS_FILE = @"Y:\#Admin\Tools\PowershellModules\Branching\BranchingModule.config";
		#endregion

		#region Properties
		public string TeamFoundationServerPath { get; private set; }
		public string MSBuildExePath { get; private set; }
		public string TFExePath { get; private set; }
		public string AdeNetExePath { get; private set; }
		public string BuildConfigurationUrl { get; private set; }
		public string DumpRepositoryPath { get; private set; }
		public string LogfilePath { get; private set; }
		public string AppConfigServerPath { get; private set; }
		public string TempDirectory { get; private set; }
		public string SQLScriptPath { get; private set; }
		public string SQLConnectionString { get; private set; }

		private Dictionary<string, ITeamProjectSettings> TeamprojectSettings { get; set; }
		#endregion

		#region Constructors
		internal Settings(SettingsDTO settingsDTO)
		{
			this.TeamFoundationServerPath = settingsDTO.TfsPath;
			this.MSBuildExePath = settingsDTO.MSBuildPath;
			this.TFExePath = settingsDTO.TFPath;
			this.AdeNetExePath = settingsDTO.AdeNetPath;
			this.BuildConfigurationUrl = settingsDTO.BuildConfigurationUrl;
			this.DumpRepositoryPath = settingsDTO.DumpRepositoryPath;
			this.LogfilePath = settingsDTO.LogfilePath;
			this.AppConfigServerPath = settingsDTO.AppConfigServerPath;
			this.TempDirectory = settingsDTO.TempDirectory;
			this.SQLScriptPath = settingsDTO.SQLScriptPath;
			this.SQLConnectionString = settingsDTO.SQLConnectionString;

			this.TeamprojectSettings = new Dictionary<string, ITeamProjectSettings>(StringComparer.OrdinalIgnoreCase);
			foreach(string strTeamproject in settingsDTO.Teamprojects.Keys)
			{
				this.TeamprojectSettings.Add(strTeamproject, new TeamProjectSettings(settingsDTO.Teamprojects[strTeamproject]));
			}
		}
		#endregion

		#region Publics
		public ITeamProjectSettings GetTeamProjectSettings(string strTeamproject)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");

			return this.TeamprojectSettings[strTeamproject];
		}

		public bool IsSupportedTeamproject(string strTeamProject)
		{
			return this.TeamprojectSettings.ContainsKey(strTeamProject);
		}
		#endregion
	}
}