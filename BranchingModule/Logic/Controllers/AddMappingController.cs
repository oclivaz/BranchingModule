using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		public IBuildEngineService BuildEngine { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlService sourceControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IConfigFileService configFileService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(configFileService == null) throw new ArgumentNullException("configFileService");

			this.SourceControl = sourceControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.ConfigFileService = configFileService;
		}
		#endregion

		#region Publics
		public void Process(BranchInfo branch)
		{
			this.SourceControl.CreateMapping(branch);

			this.AdeNet.InstallPackages(branch);

			this.ConfigFileService.CreateIndivConfig(branch);

			this.SourceControl.CreateAppConfig(branch);

			this.AdeNet.BuildWebConfig(branch);

			this.BuildEngine.Build(branch);
		}
		#endregion
	}
}