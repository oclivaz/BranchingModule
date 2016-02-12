using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private IAblageService Ablage { get; set; }
		private IConvention Convention { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IDatabaseService Database { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IEnvironmentService Environment { get; set; }
		private IConfigFileService ConfigFile { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(IVersionControlService versionControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IConfigFileService configFileService, IDatabaseService databaseService, IAblageService ablageService, IEnvironmentService environmentService, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(configFileService == null) throw new ArgumentNullException("configFileService");
			if(ablageService == null) throw new ArgumentNullException("ablageService");
			if(environmentService == null) throw new ArgumentNullException("environmentService");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.ConfigFile = configFileService;
			this.Database = databaseService;
			this.Ablage = ablageService;
			this.Environment = environmentService;
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
			this.ConfigFile.CreateIndivConfig(branch);

			this.TextOutput.WriteVerbose("Creating App.config");
			this.ConfigFile.CreateAppConfig(branch);

			this.TextOutput.WriteVerbose("Building Web.config");
			this.AdeNet.BuildWebConfig(branch);

			this.TextOutput.WriteVerbose("Building Solution");
			this.BuildEngine.Build(branch);

			this.TextOutput.WriteVerbose("Initializing IIS");
			this.AdeNet.InitializeIIS(branch);

			this.TextOutput.WriteVerbose("Restoring Dump");
			this.Database.Restore(branch);

			this.TextOutput.WriteVerbose("Resetting Ablage");
			this.Ablage.Reset(branch);
		}

		private void OpenSolution(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Opening Solution {0}", this.Convention.GetSolutionFile(branch)));
			this.Environment.OpenSolution(branch);
		}
		#endregion
	}
}