using System;

namespace BranchingModule.Logic
{
	internal class AddReleasebranchController
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		private IDumpService Dump { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public AddReleasebranchController(ISourceControlService sourceControlService, IDumpService dumpService, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.SourceControl = sourceControlService;
			this.Dump = dumpService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void AddReleasebranch(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Creating Branch");
			this.SourceControl.CreateBranch(branch);

			this.TextOutput.WriteVerbose("Creating Dump on Buildserver");
			this.Dump.InstallBuildserverDump(branch);

			//this.TextOutput.WriteVerbose("Creating Build configuration");
			//this.SourceControl.CreateBuildConfiguration(branch);
		}
		#endregion
	}
}