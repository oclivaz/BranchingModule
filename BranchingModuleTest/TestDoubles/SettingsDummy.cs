using System;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	public class SettingsDummy : ISettings
	{
		#region Properties
		public static string TeamFoundationServerPath
		{
			get { return "SettingsDummy.TeamFoundationServerPath"; }
		}

		public static string MSBuildExePath
		{
			get { return "SettingsDummy.MSBuildExePath"; }
		}

		public static string TFExePath
		{
			get { return "SettingsDummy.TFExePath"; }
		}

		public static string AdeNetExePath
		{
			get { return "SettingsDummy.AdeNetExePath"; }
		}

		public static string BuildConfigurationUrl
		{
			get { return "SettingsDummy.BuildConfigurationUrl"; }
		}

		public static string DumpRepositoryPath
		{
			get { return "SettingsDummy.DumpRepositoryPath"; }
		}

		public static string LogfilePath
		{
			get { return "SettingsDummy.LogfilePath"; }
		}

		public static string AppConfigServerPath
		{
			get { return "SettingsDummy.AppConfigServerPath"; }
		}

		public static string TempDirectory
		{
			get { return "SettingsDummy.TempDirectory"; }
		}

		public static string SQLConnectionString
		{
			get { return "SettingsDummy.SQLConnectionString"; }
		}

		public static string SQLScriptPath
		{
			get { return "SettingsDummy.SQLScriptPath"; }
		}

		public static string[] SupportedTeamProjects
		{
			get { return new[] { "one", "two", "three" }; }
		}
		#endregion

		#region Publics
		public ITeamProjectSettings GetTeamProjectSettings(string strTeamproject)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public bool IsSupportedTeamproject(string strTeamProject)
		{
			return true;
		}
		#endregion

		#region ISettings Members
		string ISettings.MSBuildExePath
		{
			get { return MSBuildExePath; }
		}

		string ISettings.TFExePath
		{
			get { return TFExePath; }
		}

		string ISettings.AdeNetExePath
		{
			get { return AdeNetExePath; }
		}

		string ISettings.BuildConfigurationUrl
		{
			get { return BuildConfigurationUrl; }
		}

		string ISettings.DumpRepositoryPath
		{
			get { return DumpRepositoryPath; }
		}

		string ISettings.LogfilePath
		{
			get { return LogfilePath; }
		}

		string ISettings.AppConfigServerPath
		{
			get { return AppConfigServerPath; }
		}

		string ISettings.TempDirectory
		{
			get { return TempDirectory; }
		}

		string ISettings.SQLConnectionString
		{
			get { return SQLConnectionString; }
		}

		string ISettings.SQLScriptPath
		{
			get { return SQLScriptPath; }
		}

		string[] ISettings.SupportedTeamprojects
		{
			get { return SupportedTeamProjects; }
		}

		string ISettings.TeamFoundationServerPath
		{
			get { return TeamFoundationServerPath; }
		}
		#endregion
	}
}