using System;
using System.Data.SqlClient;

namespace BranchingModule.Logic
{
	internal class MssqlServerAdapter : ISQLServerAdapter
	{
		#region Properties
		private ISettings Settings { get; set; }
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

			using(SqlConnection connection = new SqlConnection(this.Settings.SQLConnectionString))
			{
				connection.Open();
				connection.ChangeDatabase(strDataBase);

				SqlCommand command = new SqlCommand(strScript, connection);
				command.ExecuteNonQuery();
			}
		}
		#endregion
	}
}