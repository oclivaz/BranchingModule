using System;

namespace BranchingModule.Logic
{
	internal class RemoveDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveDatabaseController(IDatabaseService databaseService, IAblageService ablageService, ITextOutputService textOutputService)
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
		public void RemoveDatabase(BranchInfo branch)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}