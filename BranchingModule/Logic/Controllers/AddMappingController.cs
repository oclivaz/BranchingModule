using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISourceControlAdapter SourceControl { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlAdapter sourceControlAdapter, ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.SourceControl = sourceControlAdapter;
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