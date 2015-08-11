using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace BranchingModule.Logic
{
	internal class TeamFoundationVersionControlAdapter : ITeamFoundationVersionControlAdapter
	{
		#region Fields
		private TfsTeamProjectCollection _server;
		private VersionControlServer _versionControlServer;
		private Workspace _workspace;
		#endregion

		#region Properties
		private ISettings Settings { get; set; }

		private TfsTeamProjectCollection TeamProjectCollection
		{
			get { return _server ?? (_server = CreateTeamProjectCollection()); }
		}

		private VersionControlServer VersionControlServer
		{
			get { return _versionControlServer ?? (_versionControlServer = this.TeamProjectCollection.GetService<VersionControlServer>()); }
		}

		private Workspace Workspace
		{
			get { return _workspace ?? (_workspace = this.VersionControlServer.GetWorkspace(Environment.MachineName, Environment.UserName)); }
		}
		#endregion

		#region Constructors
		public TeamFoundationVersionControlAdapter(ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.Settings = settings;
		}
		#endregion

		#region Publics
		public string[] GetServerItemsByChangeset(string strChangeset)
		{
			Changeset changeset = this.VersionControlServer.GetChangeset(int.Parse(strChangeset));

			return (from change in changeset.Changes
			        let item = change.Item
			        select item.ServerItem).ToArray();
		}

		public void CreateMapping(string strServerPath, string strLocalPath)
		{
			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			this.Workspace.CreateMapping(folder);
		}

		public void Get(string strServerPath)
		{
			GetRequest getRequest = new GetRequest(strServerPath, RecursionType.Full, VersionSpec.Latest);
			this.Workspace.Get(getRequest, GetOptions.None);
		}

		public void Get()
		{
			this.Workspace.Get();
		}

		public void DeleteMapping(string strServerPath, string strLocalPath)
		{
			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			if(this.Workspace.Folders.Contains(folder)) this.Workspace.DeleteMapping(folder);
		}

		public DateTime GetCreationTime(string strItem, string strVersionSpec)
		{
			Item item = this.VersionControlServer.GetItem(strItem, VersionSpec.ParseSingleSpec(strVersionSpec, null));
			if(item == null) throw new Exception(string.Format("Kein Checkin zur Version {0} gefunden", strVersionSpec));

			return item.CheckinDate;
		}

		public void CreateBranch(string strSourceBranch, string strTargetBranch, string strVersionSpec)
		{
			this.VersionControlServer.CreateBranch(strSourceBranch, strTargetBranch, VersionSpec.ParseSingleSpec(strVersionSpec, null));
		}

		public bool ServerItemExists(string strServerItem)
		{
			return this.VersionControlServer.ServerItemExists(strServerItem, ItemType.Any);
		}

		public void DeleteBranch(string strPath)
		{
			this.VersionControlServer.Destroy(new ItemSpec(strPath, RecursionType.Full), VersionSpec.Latest, null, DestroyFlags.Silent);
		}
		#endregion

		#region Privates
		private TfsTeamProjectCollection CreateTeamProjectCollection()
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();
			return server;
		}
		#endregion
	}
}