using System;

namespace BranchingModule.Logic
{
	internal class AdeNetService : IAdeNetService
	{
		#region Properties
		private ITextOutputService TextOutput { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AdeNetService(IFileExecutionService fileExecutionService, IConvention convention, ISettings settings, ITextOutputService textOutputService)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.FileExecution = fileExecutionService;
			this.Convention = convention;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void InstallPackages(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -deploy -development", this.Convention.GetLocalPath(branch)));
		}

		public void BuildWebConfig(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -buildwebconfig -development", this.Convention.GetLocalPath(branch)));
		}

		public void InitializeIIS(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -initializeiis -development", this.Convention.GetLocalPath(branch)));
		}

		public void CleanupIIS(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -cleanupiis -development", this.Convention.GetLocalPath(branch)));
		}

		public void CreateBuildDefinition(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Starting Internet Explorer. Add a build configuration for {0}", branch));
			this.FileExecution.StartProcess(Executables.INTERNET_EXPLORER, this.Settings.BuildConfigurationUrl);
		}
		#endregion
	}
}