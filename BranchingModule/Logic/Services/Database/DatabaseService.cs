using System;
using BranchingModule.Logic.Utils;
using SmartFormat;

namespace BranchingModule.Logic
{
	internal class DatabaseService : IDatabaseService
	{
		#region Constants
		private const string MASTER = "master";

		private const string SCRIPT_BACKUP_DATABASE = "BackupDatabase.sql";
		private const string SCRIPT_RESTORE_DATABASE = "RestoreDatabase.sql";
		private const string SCRIPT_KILL_CONNECTIONS = "KillConnections.sql";
		private const string SCRIPT_POST_RESTORE_UPDATES = "PostRestoreUpdates.sql";
		#endregion

		#region Properties
		private ISQLServerAdapter SQLServer { get; set; }
		private ITextOutputService TextOutput { get; set; }
		private IDumpRepositoryService DumpRepository { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public DatabaseService(IDumpRepositoryService dumpRepositoryService, IFileSystemAdapter fileSystemAdapter, ISQLServerAdapter sqlServerAdapter, IConvention convention, ISettings settings,
		                       ITextOutputService textOutputService)
		{
			this.DumpRepository = dumpRepositoryService;
			this.FileSystem = fileSystemAdapter;
			this.SQLServer = sqlServerAdapter;
			this.Convention = convention;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void Backup(BranchInfo branch)
		{
			this.SQLServer.ExecuteScript(GetBackupDatabaseScript(this.Convention.GetLocalDump(branch), this.Settings.GetTeamProjectSettings(branch.TeamProject).LocalDB), MASTER);
		}

		public void Backup(BranchInfo branch, string strFile)
		{
			if(strFile == null) throw new ArgumentNullException("strFile");

			this.SQLServer.ExecuteScript(GetBackupDatabaseScript(strFile, this.Settings.GetTeamProjectSettings(branch.TeamProject).LocalDB), MASTER);
		}

		public void Restore(BranchInfo branch)
		{
			GetDump(branch);

			Restore(this.Convention.GetLocalDump(branch), this.Settings.GetTeamProjectSettings(branch.TeamProject).LocalDB, branch);
		}

		public void Restore(BranchInfo branch, string strFile)
		{
			if(strFile == null) throw new ArgumentNullException("strFile");

			Restore(strFile, this.Settings.GetTeamProjectSettings(branch.TeamProject).LocalDB, branch);
		}

		public void InstallBuildserverDump(BranchInfo branch)
		{
			if(this.FileSystem.Exists(this.Convention.GetBuildserverDump(branch)))
			{
				this.TextOutput.WriteVerbose("Dump already exists on buildserver. Skipping...");
				return;
			}

			this.DumpRepository.CopyDump(branch, this.Convention.GetBuildserverDump(branch));
		}

		public void DeleteLocalDump(BranchInfo branch)
		{
			if(this.FileSystem.Exists(this.Convention.GetLocalDump(branch)))
			{
				this.FileSystem.DeleteFile(this.Convention.GetLocalDump(branch));
			}
		}
		#endregion

		#region Privates
		private void GetDump(BranchInfo branch)
		{
			string strBuildServerDump = this.Convention.GetBuildserverDump(branch);
			string strLocalDump = this.Convention.GetLocalDump(branch);

			if(!this.FileSystem.Exists(strLocalDump))
			{
				if(this.FileSystem.Exists(strBuildServerDump))
				{
					this.TextOutput.WriteVerbose(string.Format("Getting {0}", strBuildServerDump));
					this.FileSystem.Copy(strBuildServerDump, this.Convention.GetLocalDump(branch));
				}
				else
				{
					this.TextOutput.WriteVerbose("Getting Dump from Repository");
					this.DumpRepository.CopyDump(branch, this.Convention.GetLocalDump(branch));
				}

				return;
			}

			if(this.FileSystem.Exists(strBuildServerDump) && this.FileSystem.GetFileInfo(strBuildServerDump).ModificationTime > this.FileSystem.GetFileInfo(strLocalDump).ModificationTime)
			{
				this.TextOutput.WriteVerbose(string.Format("Getting {0}", strBuildServerDump));
				this.FileSystem.Copy(strBuildServerDump, this.Convention.GetLocalDump(branch));
			}
			else
			{
				this.TextOutput.WriteVerbose(string.Format("Local Dump is up to Date"));
			}
			
		}

		private void Restore(string strDump, string strDB, BranchInfo branch)
		{
			if(strDump == null) throw new ArgumentNullException("strDump");
			if(strDB == null) throw new ArgumentNullException("strDB");

			Retry.Do(() =>
			         {
				         this.TextOutput.WriteVerbose(string.Format("Restoring {0} into {1}", strDump, strDB));

				         this.SQLServer.ExecuteScript(GetKillConnectionsScript(strDB), MASTER);
				         this.SQLServer.ExecuteScript(GetRestoreDatabaseScript(strDump, strDB), MASTER);
				         this.SQLServer.ExecuteScript(GetPostRestoreScript(strDB, branch), strDB);
			         }, new TimeSpan(0, 0, 0, 0, this.Settings.RetryInterval));
		}

		private string GetKillConnectionsScript(string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_KILL_CONNECTIONS);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Database = strDB });
		}

		private string GetBackupDatabaseScript(string strDump, string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_BACKUP_DATABASE);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Dump = strDump, Database = strDB });
		}

		private string GetRestoreDatabaseScript(string strDump, string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_RESTORE_DATABASE);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Dump = strDump, Database = strDB });
		}

		private string GetPostRestoreScript(string strDB, BranchInfo branch)
		{
			string strScriptPath = GetScriptPath(SCRIPT_POST_RESTORE_UPDATES);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new
			                                                                {
				                                                                Database = strDB,
				                                                                Host = Environment.MachineName,
																				ApplicationName = this.Convention.GetApplicationName(branch),
																				AblagePath = this.Convention.GetAblagePath(branch)
			                                                                });
		}

		private string GetScriptPath(string strScriptName)
		{
			return string.Format(@"{0}\{1}", this.Settings.SQLScriptPath, strScriptName);
		}
		#endregion
	}
}