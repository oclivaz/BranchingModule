using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace BranchingModule.Logic
{
	internal class TeamFoundationService : ISourceControlService
	{
		#region Fields
		private TfsTeamProjectCollection _server;
		private VersionControlServer _versionControlServer;
		private Workspace _workspace;
		#endregion

		#region Properties
		public ITextOutputService TextOutput { get; set; }

		private IConvention Convention { get; set; }

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
		public TeamFoundationService(IConvention convention, ISettings settings, ITextOutputService textOutputService)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.Convention = convention;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void CreateMapping(BranchInfo branch)
		{
			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			this.Workspace.CreateMapping(folder);

			GetRequest getRequest = new GetRequest(strServerPath, RecursionType.Full, VersionSpec.Latest);
			this.Workspace.Get(getRequest, GetOptions.None);
		}

		public void DeleteMapping(BranchInfo branch)
		{
			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			if(this.Workspace.Folders.Contains(folder)) this.Workspace.DeleteMapping(folder);
			this.Workspace.Get();
		}

		public void CreateAppConfig(BranchInfo branch)
		{
			string strLocalPath = string.Format(@"{0}\Web\app.config", this.Convention.GetLocalPath(branch));

			DownloadFile(this.Settings.AppConfigServerPath, strLocalPath);
		}

		public DateTime GetCreationTime(BranchInfo branch)
		{
			VersionSpec versionSpec = GetVersionSpec(branch);

			Item nuspecFileItem = this.VersionControlServer.GetItem(string.Format(@"{0}/{1}.nuspec", this.Convention.GetServerPath(BranchInfo.Main(branch.TeamProject)), branch.TeamProject), versionSpec);
			if(nuspecFileItem == null) throw new Exception(string.Format("Kein Checkin zum Label {0} gefunden", GetLabel(branch)));

			return nuspecFileItem.CheckinDate;
		}

		public void CreateBranch(BranchInfo branch)
		{
			string strTargetBranch = this.Convention.GetServerPath(branch);

			if(this.VersionControlServer.ServerItemExists(strTargetBranch, ItemType.Any))
			{
				this.TextOutput.WriteVerbose(string.Format("Branch {0} already exists. Skipping...", strTargetBranch));
				return;
			}

			string strSourceBranch = this.Convention.GetServerPath(BranchInfo.Main(branch.TeamProject));

			VersionSpec versionByLabel = GetVersionSpec(branch);
			this.VersionControlServer.CreateBranch(strSourceBranch, strTargetBranch, versionByLabel);
		}

		public void DeleteBranch(BranchInfo branch)
		{
			string strBranchBasePath = this.Convention.GetServerBasePath(branch);

			if(!this.VersionControlServer.ServerItemExists(strBranchBasePath, ItemType.Any))
			{
				this.TextOutput.WriteVerbose(string.Format("{0} is already deleted. Skipping...", strBranchBasePath));
				return;
			}

			this.TextOutput.WriteVerbose(string.Format("Destroying {0}", strBranchBasePath));
			this.VersionControlServer.Destroy(new ItemSpec(strBranchBasePath, RecursionType.Full), VersionSpec.Latest, null, DestroyFlags.Silent);
		}

		public void CreateBuildConfiguration(BranchInfo branch)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Privates
		private TfsTeamProjectCollection CreateTeamProjectCollection()
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();
			return server;
		}

		private VersionSpec GetVersionSpec(BranchInfo branch)
		{
			return VersionSpec.ParseSingleSpec(string.Format("L{0}", GetLabel(branch)), null);
		}

		private string GetLabel(BranchInfo branch)
		{
			return string.Format("{0}.0", branch.Name);
		}

		private void DownloadFile(string strServerpath, string strLocalpath)
		{
			if(strServerpath == null) throw new ArgumentNullException("strServerpath");
			if(strLocalpath == null) throw new ArgumentNullException("strLocalpath");

			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			versioncontrol.DownloadFile(strServerpath, strLocalpath);
		}
		#endregion
	}
}