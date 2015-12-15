namespace BranchingModule.Logic
{
	internal class BackupDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		#endregion

		#region Constructors
		public BackupDatabaseController(IDatabaseService databaseService)
		{
			this.Database = databaseService;
		}
		#endregion

		#region Publics
		public void BackupDatabase(BranchInfo branch)
		{
			this.Database.Backup(branch);
		}
		#endregion
	}
}