using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISourceControlAdapter SourceControl { get; set; }
		private IAdeNetAdapter AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlAdapter sourceControlAdapter, IAdeNetAdapter adeNetAdapter, IConfigFileService configFileService)
		{
			if(sourceControlAdapter == null) throw new ArgumentNullException("sourceControlAdapter");

			this.SourceControl = sourceControlAdapter;
			this.AdeNet = adeNetAdapter;
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
		}
		#endregion
	}
}