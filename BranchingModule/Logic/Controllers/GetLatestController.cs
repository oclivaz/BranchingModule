using System;

namespace BranchingModule.Logic
{
	internal class GetLatestController
	{
		#region Properties
		private IAblageService Ablage { get; set; }
		private IConvention Convention { get; set; }
		private IBuildEngineService BuildEngine { get; set; }
		private IDatabaseService Database { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IEnvironmentService Environment { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public GetLatestController(IVersionControlService versionControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IDatabaseService databaseService,
		                           IAblageService ablageService, IEnvironmentService environmentService, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(ablageService == null) throw new ArgumentNullException("ablageService");
			if(environmentService == null) throw new ArgumentNullException("environmentService");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.Database = databaseService;
			this.Ablage = ablageService;
			this.Environment = environmentService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void GetLatest(BranchInfo branch, bool bOpenSolution, bool bOpenWeb)
		{
			this.TextOutput.WriteVerbose("Getting latest Version");
			this.VersionControl.GetLatest(branch);

			this.TextOutput.WriteVerbose("Installing Packages");
			this.AdeNet.InstallPackages(branch);

			this.TextOutput.WriteVerbose("Restoring Dump");
			this.Database.Restore(branch);

			this.TextOutput.WriteVerbose("Building Solution");
			this.BuildEngine.Build(branch);

			this.TextOutput.WriteVerbose("Resetting Ablage");
			this.Ablage.Reset(branch);

			if(bOpenSolution)
			{
				OpenSolution(branch);
			}

			if(bOpenWeb)
			{
				this.Environment.OpenWeb(branch);
			}
		}
		#endregion

		#region Privates
		private void OpenSolution(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Opening Solution {0}", this.Convention.GetSolutionFile(branch)));
			this.Environment.OpenSolution(branch);
		}
		#endregion
	}
}