using System.Collections.Generic;

namespace BranchingModule.Logic
{
	internal class SettingsDTO
	{
		#region Properties
		public string TfsPath { get; set; }
		public string MSBuildPath { get; set; }
		public string TFPath { get; set; }
		public string AdeNetPath { get; set; }
		public string BuildConfigurationUrl { get; set; }
		public string DumpRepositoryPath { get; set; }
		public string LogfilePath { get; set; }

		public Dictionary<string, TeamProjectSettingsDTO> Teamprojects { get; set; }
		#endregion
	}
}