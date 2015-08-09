using System;

namespace BranchingModule.Logic
{
	internal class AdeNetService : IAdeNetService
	{
		#region Properties
		private IFileSystemService FileSystem { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AdeNetService(IFileSystemService fileSystemService, IConvention convention, ISettings settings)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.FileSystem = fileSystemService;
			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void InstallPackages(BranchInfo branch)
		{
			this.FileSystem.ExecuteInCmd(GetAdeNetExe(), string.Format("-workingdirectory {0} -deploy -development", this.Convention.GetLocalPath(branch)));
		}

		public void BuildWebConfig(BranchInfo branch)
		{
			this.FileSystem.ExecuteInCmd(GetAdeNetExe(), string.Format("-workingdirectory {0} -buildwebconfig -development", this.Convention.GetLocalPath(branch)));
		}

		public void InitializeIIS(BranchInfo branch)
		{
			this.FileSystem.ExecuteInCmd(GetAdeNetExe(), string.Format("-workingdirectory {0} -initializeiis -development", this.Convention.GetLocalPath(branch)));
		}
		#endregion

		#region Privates
		private string GetAdeNetExe()
		{
			return string.Format(@"{0}\AdeNet.exe", this.Settings.AdeNetExePath);
		}
		#endregion
	}
}