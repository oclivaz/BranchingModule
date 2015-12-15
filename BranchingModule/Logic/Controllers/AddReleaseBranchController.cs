using System;

namespace BranchingModule.Logic
{
	internal class AddReleasebranchController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IDatabaseService Database { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddReleasebranchController(IVersionControlService versionControlService, IDatabaseService databaseService, IAdeNetService adeNetService, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.VersionControl = versionControlService;
			this.Database = databaseService;
			this.AdeNet = adeNetService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void AddReleasebranch(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Creating Branch");
			this.VersionControl.CreateBranch(branch);

			this.TextOutput.WriteVerbose("Creating Dump on Buildserver");
			this.Database.InstallBuildserverDump(branch);

			this.TextOutput.WriteVerbose("Creating Build configuration");
			this.AdeNet.CreateBuildDefinition(branch);
		}
		#endregion
	}
}