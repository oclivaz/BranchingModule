using System;

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

		public void BackupDatabase(BranchInfo branch, string strFile)
		{
			if(strFile == null) throw new ArgumentNullException("strFile");

			this.Database.Backup(branch, strFile);
		}
		#endregion
	}
}