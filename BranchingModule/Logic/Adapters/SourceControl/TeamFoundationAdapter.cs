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
		public void CreateMapping(string strTeamproject, string strBranch)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			Workspace workspace = versioncontrol.GetWorkspace(Environment.MachineName, Environment.UserName);

			string strLocalPath = this.Convention.GetLocalPath(strTeamproject, strBranch);
			string strServerPath = this.Convention.GetServerPath(strTeamproject, strBranch);

			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			workspace.CreateMapping(folder);

			GetRequest getRequest = new GetRequest(strServerPath, RecursionType.Full, VersionSpec.Latest);
			workspace.Get(getRequest, GetOptions.None);
		}
		#endregion
	}
}