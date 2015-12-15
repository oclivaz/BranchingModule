namespace BranchingModule.Logic
{
	internal class RestoreDatabaseController
	{
		#region Properties
		private IDatabaseService Database { get; set; }
		#endregion

		#region Constructors
		public RestoreDatabaseController(IDatabaseService databaseService)
		{
			this.Database = databaseService;
		}
		#endregion

		#region Publics
		public void RestoreDatabase(BranchInfo branch)
		{
			this.Database.Restore(branch);
		}
		#endregion
	}
}