using System;
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
		public ISQLServerService SQLServer { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public DumpService(IDumpRepositoryService dumpRepositoryService, IFileSystemService fileSystemService, ISQLServerService sqlServerService, IConvention convention, ISettings settings)
		{
			this.DumpRepository = dumpRepositoryService;
			this.FileSystem = fileSystemService;
			this.SQLServer = sqlServerService;
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

			string strBuildServerDump = this.Convention.GetBuildserverDump(branch);

			if(this.FileSystem.Exists(strBuildServerDump)) this.FileSystem.Copy(strBuildServerDump, strLocalDump);
			else this.DumpRepository.CopyDump(branch, strLocalDump);
		}

		private void RestoreDump(string strDump, string strDB)
		{
			if(strDump == null) throw new ArgumentNullException("strDump");
			if(strDB == null) throw new ArgumentNullException("strDB");

			this.SQLServer.ExecuteScript(GetKillConnectionsScript(strDB), MASTER);
			this.SQLServer.ExecuteScript(GetRestoreDatabaseScript(strDump, strDB), MASTER);
			this.SQLServer.ExecuteScript(GetPostRestoreScript(strDB), strDB);
		}

		private string GetKillConnectionsScript(string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_KILL_CONNECTIONS);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Database = strDB });
		}

		private string GetRestoreDatabaseScript(string strDump, string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_RESTORE_DATABASE);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Dump = strDump, Database = strDB });
		}

		private string GetPostRestoreScript(string strDB)
		{
			string strScriptPath = GetScriptPath(SCRIPT_POST_RESTORE_UPDATES);
			return Smart.Format(this.FileSystem.ReadAllText(strScriptPath), new { Database = strDB });
		}

		private string GetScriptPath(string strScriptName)
		{
			return string.Format(@"{0}\{1}", this.Settings.SQLScriptPath, strScriptName);
		}
		#endregion
	}
}