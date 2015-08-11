using System;

namespace BranchingModule.Logic
{
	internal class MergeChangesetController
	{
		#region Properties
		private IBuildEngineService BuildEngine { get; set; }
		private IDumpService Dump { get; set; }
		private ISourceControlService SourceControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IConfigFileService ConfigFileService { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public MergeChangesetController(ISourceControlService sourceControlService, IAdeNetService adeNetService, IBuildEngineService buildEngineService, IConfigFileService configFileService,
		                                IDumpService dumpService, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(buildEngineService == null) throw new ArgumentNullException("buildEngineService");
			if(configFileService == null) throw new ArgumentNullException("configFileService");

			this.SourceControl = sourceControlService;
			this.AdeNet = adeNetService;
			this.BuildEngine = buildEngineService;
			this.ConfigFileService = configFileService;
			this.Dump = dumpService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void MergeChangeset(string strTeamproject, string strChangeset, string[] targetBranches)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");
			if(targetBranches == null) throw new ArgumentNullException("targetBranches");

			BranchInfo branch = this.SourceControl.GetBranchInfo(strChangeset);
		}
		#endregion
	}
}