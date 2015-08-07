using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BranchingModule.Logic
{
	internal class TeamFoundationAdapter : ISourceControlAdapter
	{
		#region Properties
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public TeamFoundationAdapter(IConvention convention, ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void CreateMapping(string localPath, string serverPath)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			

		}
		#endregion
	}
}