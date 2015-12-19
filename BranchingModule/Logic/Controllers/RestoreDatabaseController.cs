using System;

namespace BranchingModule.Logic
{
	internal class RestoreDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		public IAblageService Ablage { get; set; }
		public IFileSystemAdapter FileSystemAdapter { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RestoreDatabaseController(IDatabaseService databaseService, IAblageService ablageService, ITextOutputService textOutputService)
		{
			if(databaseService == null) throw new ArgumentNullException("databaseService");
			if(ablageService == null) throw new ArgumentNullException("ablageService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.Database = databaseService;
			this.Ablage = ablageService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RestoreDatabase(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Restoring Database");
			this.Database.Restore(branch);

			this.TextOutput.WriteVerbose("Preparing Ablage");
			this.Ablage.Reset(branch);
		}

		public void RestoreDatabase(BranchInfo branch, string strFile)
		{
			this.TextOutput.WriteVerbose("Restoring Database");
			this.Database.Restore(branch, strFile);

			this.TextOutput.WriteVerbose("Preparing Ablage");
			this.Ablage.Reset(branch);
		}
		#endregion
	}
}