using System;

namespace BranchingModule.Logic
{
	internal class GetLatestController
	{
		#region Properties
		public IFileExecutionService FileExecution { get; set; }
		public IConvention Convention { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IDatabaseService Database { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public GetLatestController(IVersionControlService versionControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService,
								   IDatabaseService databaseService, IFileExecutionService fileExecutionService, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(fileExecutionService == null) throw new ArgumentNullException("fileExecutionService");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.Database = databaseService;
			this.FileExecution = fileExecutionService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void GetLatest(BranchInfo branch, bool bOpenSolution)
		{
			this.TextOutput.WriteVerbose("Getting latest Version");
			this.VersionControl.GetLatest(branch);

			this.TextOutput.WriteVerbose("Installing Packages");
			this.AdeNet.InstallPackages(branch);

			this.TextOutput.WriteVerbose("Restoring Dump");
			this.Database.Restore(branch);

			this.TextOutput.WriteVerbose("Building Solution");
			this.BuildEngine.Build(branch);

			if(bOpenSolution)
			{
				OpenSolution(branch);
			}
		}
		#endregion

		#region Privates
		private void OpenSolution(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Opening Solution {0}", this.Convention.GetSolutionFile(branch)));
			this.FileExecution.StartProcess(Executables.EXPLORER, this.Convention.GetSolutionFile(branch));
		}
		#endregion
	}
}