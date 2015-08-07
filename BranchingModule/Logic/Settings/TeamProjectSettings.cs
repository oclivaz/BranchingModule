using System;

namespace BranchingModule.Logic
{
	internal class TeamProjectSettings : ITeamProjectSettings
	{
		#region Properties
		public string LocalDB { get; private set; }
		public string RefDB { get; private set; }
		#endregion

		#region Constructors
		public TeamProjectSettings(TeamProjectSettingsDTO settingsDTO)
		{
			if(settingsDTO == null) throw new ArgumentNullException("settingsDTO");

			this.LocalDB = settingsDTO.LocalDB;
			this.RefDB = settingsDTO.RefDB;
		}
		#endregion
	}
}