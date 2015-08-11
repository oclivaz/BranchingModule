using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		public IFileExecutionService FileExecution { get; set; }
		public IConvention Convention { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IDumpService Dump { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(IVersionControlService versionControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IConfigFileService configFileService,
		                            IDumpService dumpService, IFileExecutionService fileExecutionService, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(configFileService == null) throw new ArgumentNullException("configFileService");
			if(fileExecutionService == null) throw new ArgumentNullException("fileExecutionService");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.ConfigFileService = configFileService;
			this.Dump = dumpService;
			this.FileExecution = fileExecutionService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void AddMapping(BranchInfo branch, bool bMinimal, bool bOpenSolution)
		{
			this.TextOutput.WriteVerbose("Creating Mapping");
			this.VersionControl.CreateMapping(branch);

			if(!bMinimal) CreateRunningEnvironment(branch);

			if(bOpenSolution)
			{
				OpenSolution(branch);
			}
		}
		#endregion

		#region Privates
		private void CreateRunningEnvironment(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Installing Packages");
			this.AdeNet.InstallPackages(branch);

			this.TextOutput.WriteVerbose("Creating Indiv.config");
			this.ConfigFileService.CreateIndivConfig(branch);

			this.TextOutput.WriteVerbose("Creating App.config");
			this.VersionControl.CreateAppConfig(branch);

			this.TextOutput.WriteVerbose("Building Web.config");
			this.AdeNet.BuildWebConfig(branch);

			this.TextOutput.WriteVerbose("Building Solution");
			this.BuildEngine.Build(branch);

			this.TextOutput.WriteVerbose("Initializing IIS");
			this.AdeNet.InitializeIIS(branch);

			this.TextOutput.WriteVerbose("Restoring Dump");
			this.Dump.RestoreDump(branch);
		}

		private void OpenSolution(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Opening Solution {0}", this.Convention.GetSolutionFile(branch)));
			this.FileExecution.StartProcess(Executables.EXPLORER, this.Convention.GetSolutionFile(branch));
		}
		#endregion
	}
}