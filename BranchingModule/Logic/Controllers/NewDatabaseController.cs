using System;

namespace BranchingModule.Logic
{
	internal class NewDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public NewDatabaseController(IDatabaseService databaseService, IAblageService ablageService, ITextOutputService textOutputService)
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
		public void CreateDatabase(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Creating Database");
			this.Database.Create(branch);

			this.TextOutput.WriteVerbose("Preparing Ablage");
			this.Ablage.Reset(branch);
		}
		#endregion
	}
}