﻿using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BranchingModule.Logic
{
	internal class TeamFoundationService : ISourceControlService
	{
		#region Properties
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public TeamFoundationService(IConvention convention, ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void CreateMapping(BranchInfo branch)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			Workspace workspace = versioncontrol.GetWorkspace(Environment.MachineName, Environment.UserName);

			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			workspace.CreateMapping(folder);

			GetRequest getRequest = new GetRequest(strServerPath, RecursionType.Full, VersionSpec.Latest);
			workspace.Get(getRequest, GetOptions.None);
		}

		public void DeleteMapping(BranchInfo branch)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			Workspace workspace = versioncontrol.GetWorkspace(Environment.MachineName, Environment.UserName);

			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			WorkingFolder folder = new WorkingFolder(strServerPath, strLocalPath);

			if(workspace.Folders.Contains(folder)) workspace.DeleteMapping(folder);
			workspace.Get();
		}

		public void CreateAppConfig(BranchInfo branch)
		{
			string strLocalPath = string.Format(@"{0}\Web\app.config", this.Convention.GetLocalPath(branch));

			DownloadFile(this.Settings.AppConfigServerPath, strLocalPath);
		}

		public DateTime GetCreationTime(BranchInfo branch)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			VersionSpec versionSpec = GetVersionSpec(branch);

			Item nuspecFileItem = versioncontrol.GetItem(string.Format(@"{0}/{1}.nuspec", this.Convention.GetServerPath(BranchInfo.Main(branch.TeamProject)), branch.TeamProject), versionSpec);
			if(nuspecFileItem == null) throw new Exception(string.Format("Kein Checkin zum Label {0} gefunden", GetLabel(branch)));

			return nuspecFileItem.CheckinDate;
		}

		public void CreateBranch(BranchInfo branch)
		{
			TfsTeamProjectCollection server = new TfsTeamProjectCollection(new Uri(Settings.TeamFoundationServerPath));
			server.Authenticate();

			string strTargetBranch = this.Convention.GetServerPath(branch);

			VersionControlServer versioncontrol = server.GetService<VersionControlServer>();
			if(versioncontrol.ServerItemExists(strTargetBranch, ItemType.Any)) return;

			string strSourceBranch = this.Convention.GetServerPath(BranchInfo.Main(branch.TeamProject));

			VersionSpec versionByLabel = GetVersionSpec(branch);
			versioncontrol.CreateBranch(strSourceBranch, strTargetBranch, versionByLabel);
		}
		#endregion

		#region Privates
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