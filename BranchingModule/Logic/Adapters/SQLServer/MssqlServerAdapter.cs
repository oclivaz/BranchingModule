using System;
using System.Data.SqlClient;

namespace BranchingModule.Logic
{
	internal class MssqlServerAdapter : ISQLServerAdapter, IDisposable
	{
		#region Fields
		private SqlConnection _sqlConnection;
		#endregion

		#region Properties
		private ISettings Settings { get; set; }

		private SqlConnection SqlConnection
		{
			get { return _sqlConnection ?? (_sqlConnection = OpenConnection()); }
		}
		#endregion

		#region Constructors
		public MssqlServerAdapter(ISettings settings)
		{
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void ExecuteScript(string strScript, string strDataBase)
		{
			if(strScript == null) throw new ArgumentNullException("strScript");

			this.SqlConnection.ChangeDatabase(strDataBase);

			SqlCommand command = new SqlCommand(strScript, this.SqlConnection);
			command.ExecuteNonQuery();
		}

		public void Dispose()
		{
			this.SqlConnection.Close();
			this.SqlConnection.Dispose();
		}
		#endregion

		#region Privates
		private SqlConnection OpenConnection()
		{
			SqlConnection connection = new SqlConnection(this.Settings.SQLConnectionString);
			connection.Open();

			return connection;
		}
		#endregion
	}
}