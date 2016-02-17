using System;

namespace BranchingModule.Logic
{
	internal class RemoveMappingController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		private IEnvironmentService Environment { get; set; }
		private IConvention Convention { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveMappingController(IVersionControlService versionControlService, IAdeNetService adeNetService, IFileSystemAdapter fileSystemAdapter, IDatabaseService databaseService, IAblageService ablageService, IEnvironmentService environmentService, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(fileSystemAdapter == null) throw new ArgumentNullException("fileSystemAdapter");
			if(databaseService == null) throw new ArgumentNullException("databaseService");
			if(ablageService == null) throw new ArgumentNullException("ablageService");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.FileSystem = fileSystemAdapter;
			this.Database = databaseService;
			this.Ablage = ablageService;
			this.Environment = environmentService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveMapping(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Mapping");
			this.VersionControl.DeleteMapping(branch);

			this.TextOutput.WriteVerbose("Cleaning up IIS");
			this.AdeNet.CleanupIIS(branch);

			this.TextOutput.WriteVerbose("Resetting IIS");
			this.Environment.ResetLocalWebserver();

			this.TextOutput.WriteVerbose("Deleting Solution");
			this.FileSystem.DeleteDirectory(this.Convention.GetLocalPath(branch));

			this.TextOutput.WriteVerbose("Removing local dump file");
			this.Database.DeleteLocalDump(branch);

			this.TextOutput.WriteVerbose("Removing Ablage");
			this.Ablage.Remove(branch);
		}
		#endregion
	}
}