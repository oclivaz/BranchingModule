using System;
using System.Data;
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

		private ITextOutputService TextOutput { get; set; }

		private SqlConnection SqlConnection
		{
			get
			{
				if(_sqlConnection != null && _sqlConnection.State == ConnectionState.Open) return _sqlConnection;
				if(_sqlConnection != null) _sqlConnection.Dispose();

				return (_sqlConnection = OpenConnection());
			}
		}
		#endregion

		#region Constructors
		public MssqlServerAdapter(ISettings settings, ITextOutputService textOutput)
		{
			this.Settings = settings;
			this.TextOutput = textOutput;
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
			this.TextOutput.WriteVerbose(string.Format("Creating new Connection: {0}", this.Settings.SQLConnectionString));

			SqlConnection connection = new SqlConnection(this.Settings.SQLConnectionString);
			connection.Open();

			return connection;
		}
		#endregion
	}
}