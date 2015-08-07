using System;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class SettingsDummy : ISettings
	{
		#region Properties
		public string TeamFoundationServerPath
		{
			get { return "bullshit"; }
		}

		public string MSBuildExePath
		{
			get { return "bullshit"; }
		}
		public string TFExePath
		{
			get { return "bullshit"; }
		}
		public string AdeNetExePath
		{
			get { return "bullshit"; }
		}
		public string BuildConfigurationUrl
		{
			get { return "bullshit"; }
		}
		public string DumpRepositoryPath
		{
			get { return "bullshit"; }
		}
		public string LogfilePath
		{
			get { return "bullshit"; }
		}

		public string AppConfigServerPath 
		{
			get { return "bullshit"; }
		}
		#endregion

		#region Publics
		public ITeamProjectSettings GetTeamProjectSettings(string strTeamproject)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}