using System;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace BranchingModule.Logic
{
	internal class TeamFoundationService : IVersionControlService
	{
		#region Properties
		public ITextOutputService TextOutput { get; set; }

		private IConvention Convention { get; set; }

		private ISettings Settings { get; set; }

		private ITeamFoundationVersionControlAdapter VersionControl { get; set; }
		#endregion

		#region Constructors
		public TeamFoundationService(ITeamFoundationVersionControlAdapter versoControlAdapter, IConvention convention, ISettings settings, ITextOutputService textOutputService)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControl = versoControlAdapter;
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

			this.VersionControl.CreateMapping(strServerPath, strLocalPath);
			this.VersionControl.Get(strServerPath);
		}

		public void DeleteMapping(BranchInfo branch)
		{
			string strLocalPath = this.Convention.GetLocalPath(branch);
			string strServerPath = this.Convention.GetServerPath(branch);

			this.VersionControl.DeleteMapping(strServerPath, strLocalPath);
			this.VersionControl.Get();
		}

		public void CreateAppConfig(BranchInfo branch)
		{
			string strLocalPath = string.Format(@"{0}\Web\app.config", this.Convention.GetLocalPath(branch));

			DownloadFile(this.Settings.AppConfigServerPath, strLocalPath);
		}

		public DateTime GetCreationTime(BranchInfo branch)
		{
			string strItem = string.Format(@"{0}/{1}.nuspec", this.Convention.GetServerPath(this.Convention.MainBranch(branch.TeamProject)), branch.TeamProject);
			string strVersionSpec = GetVersionSpec(branch);

			return this.VersionControl.GetCreationTime(strItem, strVersionSpec);
		}

		public void CreateBranch(BranchInfo branch)
		{
			string strSourceBranch = this.Convention.GetServerPath(this.Convention.MainBranch(branch.TeamProject));
			string strTargetBranch = this.Convention.GetServerPath(branch);
			string strVersionByLabel = GetVersionSpec(branch);

			if(this.VersionControl.ServerItemExists(strTargetBranch))
			{
				this.TextOutput.WriteVerbose(string.Format("Branch {0} already exists. Skipping...", strTargetBranch));
				return;
			}

			this.VersionControl.CreateBranch(strSourceBranch, strTargetBranch, strVersionByLabel);
		}

		public void DeleteBranch(BranchInfo branch)
		{
			string strBranchBasePath = this.Convention.GetServerBasePath(branch);

			if(!this.VersionControl.ServerItemExists(strBranchBasePath))
			{
				this.TextOutput.WriteVerbose(string.Format("{0} is already deleted. Skipping...", strBranchBasePath));
				return;
			}

			this.TextOutput.WriteVerbose(string.Format("Destroying {0}", strBranchBasePath));
			this.VersionControl.DeleteBranch(strBranchBasePath);
		}

		public BranchInfo GetBranchInfo(string strChangeset)
		{
			var affectedBranches = (from item in this.VersionControl.GetServerItemsByChangeset(strChangeset)
			                        select this.Convention.GetBranchInfoByServerPath(item)).Distinct();

			return affectedBranches.Single();
		}
		#endregion

		#region Privates
		private string GetVersionSpec(BranchInfo branch)
		{
			return string.Format("L{0}", GetLabel(branch));
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