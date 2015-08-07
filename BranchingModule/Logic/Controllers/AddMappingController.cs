using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void Process(string strTeamProject, string strBranch)
		{
			



		}
		#endregion
	}
}