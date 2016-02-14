namespace BranchingModule.Logic
{
	internal class TeamProjectSettingsDTO
	{
		#region Properties
		public string LocalDB { get; set; }
		public string RefDB { get; set; }
		public string[] AditionalReferences { get; set; }
		public string AppConfigPath { get; set; }
		#endregion
	}
}