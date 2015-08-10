using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private IBuildEngineService BuildEngine { get; set; }
		private IDumpService Dump { get; set; }
		private ISourceControlService SourceControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlService sourceControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IConfigFileService configFileService, IDumpService dumpService, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(configFileService == null) throw new ArgumentNullException("configFileService");

			this.SourceControl = sourceControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.ConfigFileService = configFileService;
			this.Dump = dumpService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void AddMapping(BranchInfo branch, bool bMinimal)
		{
			this.TextOutput.WriteVerbose("Creating Mapping");
			this.SourceControl.CreateMapping(branch);

			if(bMinimal) return;

			this.TextOutput.WriteVerbose("Installing Packages");
			this.AdeNet.InstallPackages(branch);

			this.TextOutput.WriteVerbose("Creating Indiv.config");
			this.ConfigFileService.CreateIndivConfig(branch);

			this.TextOutput.WriteVerbose("Creating App.config");
			this.SourceControl.CreateAppConfig(branch);

			this.TextOutput.WriteVerbose("Building Web.config");
			this.AdeNet.BuildWebConfig(branch);

			this.TextOutput.WriteVerbose("Building Solution");
			this.BuildEngine.Build(branch);

			this.TextOutput.WriteVerbose("Initializing IIS");
			this.AdeNet.InitializeIIS(branch);

			this.TextOutput.WriteVerbose("Restoring Dump");
			this.Dump.RestoreDump(branch);
		}
		#endregion
	}
}