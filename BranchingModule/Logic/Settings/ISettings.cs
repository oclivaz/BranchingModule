﻿namespace BranchingModule.Logic
{
	public interface ISettings
	{
		string TeamFoundationServerPath { get; }
		string MSBuildExePath { get; }
		string TFExePath { get; }
		string AdeNetExePath { get; }
		string BuildConfigurationUrl { get; }
		string DumpRepositoryPath { get; }
		string LogfilePath { get; }
		string TempDirectory { get; }
		string SQLConnectionString { get; }
		string SQLScriptPath { get; }
		string[] SupportedTeamprojects { get; }
		int RetryInterval { get; }

		ITeamProjectSettings GetTeamProjectSettings(string strTeamproject);
		bool IsSupportedTeamproject(string strTeamProject);
	}
}