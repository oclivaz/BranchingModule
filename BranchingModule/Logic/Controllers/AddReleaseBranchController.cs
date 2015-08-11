using System;

namespace BranchingModule.Logic
{
	internal class AddReleasebranchController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IDumpService Dump { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddReleasebranchController(IVersionControlService versionControlService, IDumpService dumpService, IAdeNetService adeNetService, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.VersionControl = versionControlService;
			this.Dump = dumpService;
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
			this.Dump.InstallBuildserverDump(branch);

			this.TextOutput.WriteVerbose("Creating Build configuration");
			this.AdeNet.CreateBuildDefinition(branch);
		}
		#endregion
	}
}