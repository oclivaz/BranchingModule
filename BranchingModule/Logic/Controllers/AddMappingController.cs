using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISourceControlAdapter SourceControl { get; set; }
		private IAdeNetAdapter AdeNet { get; set; }
		public IBuildEngineAdapter BuildEngine { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlAdapter sourceControlAdapter, IAdeNetAdapter adeNetAdapter, IBuildEngineAdapter buildEngineAdapter, IConfigFileService configFileService)
		{
			if(sourceControlAdapter == null) throw new ArgumentNullException("sourceControlAdapter");
			if(adeNetAdapter == null) throw new ArgumentNullException("adeNetAdapter");
			if(buildEngineAdapter == null) throw new ArgumentNullException("buildEngineAdapter");
			if(configFileService == null) throw new ArgumentNullException("configFileService");

			this.SourceControl = sourceControlAdapter;
			this.AdeNet = adeNetAdapter;
			this.BuildEngine = buildEngineAdapter;
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