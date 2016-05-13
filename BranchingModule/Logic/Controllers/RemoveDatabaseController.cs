using System;

namespace BranchingModule.Logic
{
	internal class RemoveDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		#endregion

		#region Constructors
		public RemoveDatabaseController(IDatabaseService databaseService, IAblageService ablageService)
		{
			if(databaseService == null) throw new ArgumentNullException("databaseService");
			if(ablageService == null) throw new ArgumentNullException("ablageService");

			this.Database = databaseService;
			this.Ablage = ablageService;
		}
		#endregion

		#region Publics
		public void RemoveDatabase(BranchInfo branch)
		{
			this.Database.Drop(branch);
			this.Ablage.Remove(branch);
		}
		#endregion
	}
}