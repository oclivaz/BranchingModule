using System;
using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class Settings : ISettings
	{
		#region Constants
		public static readonly string DEFAULT_SETTINGS_FILE = @"\\m-s.ch\Ablage\AkisNetBV\#Admin\Tools\PowershellModules\Branching\BranchingModule.config";
		private const int DEFAULT_RETRY_INTERVAL = 500;
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
		public int RetryInterval { get; set; }

		public string[] SupportedTeamprojects
		{
			get { return this.TeamprojectSettings.Keys.ToArray(); }
		}

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

			this.RetryInterval = DEFAULT_RETRY_INTERVAL;
		}
		#endregion

		#region Publics
		public ITeamProjectSettings GetTeamProjectSettings(string strTeamproject)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(!IsSupportedTeamproject(strTeamproject)) throw new ArgumentException(string.Format("Das Teamproject {0} wird nicht unterstützt.", strTeamproject));

			return this.TeamprojectSettings[strTeamproject];
		}

		public bool IsSupportedTeamproject(string strTeamProject)
		{
			return this.TeamprojectSettings.ContainsKey(strTeamProject);
		}

		public static string[] GetSupportetTeamProjects()
		{
			return ControllerFactory.Get<Settings>().TeamprojectSettings.Keys.ToArray();
		}
		#endregion
	}
}