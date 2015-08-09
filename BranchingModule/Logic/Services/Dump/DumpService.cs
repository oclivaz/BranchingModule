using System;
using System.Data.SqlClient;
using SmartFormat;

namespace BranchingModule.Logic
{
	internal class DumpService : IDumpService
	{
		#region Constants
		private const string MASTER = "master";
		private const string SCRIPT_RESTORE_DATABASE = "RestoreDatabase.sql";
		private const string SCRIPT_KILL_CONNECTIONS = "KillConnections.sql";
		private const string SCRIPT_POST_RESTORE_UPDATES = "PostRestoreUpdates.sql";
		#endregion

		#region Properties
		private IDumpRepositoryService DumpRepository { get; set; }
		private IFileSystemService FileSystem { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public DumpService(IDumpRepositoryService dumpRepositoryService, IFileSystemService fileSystemService, IConvention convention, ISettings settings)
		{
			this.DumpRepository = dumpRepositoryService;
			this.FileSystem = fileSystemService;
			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void RestoreDump(BranchInfo branch)
		{
			EnsureLocalDump(branch);

			RestoreDump(this.Convention.GetLocalDump(branch), this.Settings.GetTeamProjectSettings(branch.TeamProject).LocalDB);
		}
		#endregion

		#region Privates
		private void EnsureLocalDump(BranchInfo branch)
		{
			string strLocalDump = this.Convention.GetLocalDump(branch);

			if(this.FileSystem.Exists(strLocalDump)) return;

			string strBuildServerDump = this.Convention.GetBuildserverPath(branch);

			if(this.FileSystem.Exists(strBuildServerDump)) this.FileSystem.Move(strBuildServerDump, strLocalDump);
			else this.DumpRepository.CopyDump(branch, strLocalDump);
		}

		private void RestoreDump(string strDump, string strDB)
		{
			if(strDump == null) throw new ArgumentNullException("strDump");
			if(strDB == null) throw new ArgumentNullException("strDB");

			using(SqlConnection connection = new SqlConnection(this.Settings.SQLConnectionString))
			{
				connection.Open();

				ExecuteKillConnections(strDB, connection);
				ExecuteRestoreDatabase(strDump, strDB, connection);
				ExecutePostRestore(strDB, connection);
			}
		}

		private void ExecuteKillConnections(string strDB, SqlConnection connection)
		{
			string strScriptPath = GetScriptPath(SCRIPT_KILL_CONNECTIONS);
			string strScript = Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Database = strDB });

			connection.ChangeDatabase(MASTER);
			ExecuteScript(connection, strScript);
		}

		private void ExecuteRestoreDatabase(string strDump, string strDB, SqlConnection connection)
		{
			string strScriptPath = GetScriptPath(SCRIPT_RESTORE_DATABASE);
			string strScript = Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Dump = strDump, Database = strDB });

			connection.ChangeDatabase(MASTER);
			ExecuteScript(connection, strScript);
		}

		private void ExecutePostRestore(string strDB, SqlConnection connection)
		{
			string strScriptPath = GetScriptPath(SCRIPT_POST_RESTORE_UPDATES);
			string strScript = Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Database = strDB });

			connection.ChangeDatabase(strDB);
			ExecuteScript(connection, strScript);
		}

		private void ExecuteScript(SqlConnection connection, string strScript)
		{
			SqlCommand command = new SqlCommand(strScript, connection);
			command.ExecuteNonQuery();
		}

		private string GetScriptPath(string strScriptName)
		{
			return string.Format(@"{0}\{1}", this.Settings.SQLScriptPath, strScriptName);
		}
		#endregion
	}
}