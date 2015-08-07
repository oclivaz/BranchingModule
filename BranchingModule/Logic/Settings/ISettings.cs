namespace BranchingModule.Logic
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

		ITeamProjectSettings GetTeamProjectSettings(string strTeamproject);
	}
}