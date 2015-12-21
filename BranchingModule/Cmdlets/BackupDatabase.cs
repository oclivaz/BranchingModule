using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Backup, "Database")]
	public class BackupDatabase : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		internal DynamicParameter<string> File { get; set; }
		#endregion

		#region Constructors
		public BackupDatabase()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
			this.File = new DynamicParameter<string>(this, "File", false, 2);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			BackupDatabaseController controller = ControllerFactory.Get<BackupDatabaseController>();

			if(string.IsNullOrEmpty(this.File)) controller.BackupDatabase(BranchInfo.Create(this.Teamproject, this.Branch));
			else controller.BackupDatabase(BranchInfo.Create(this.Teamproject, this.Branch), this.File);
		}
		#endregion
	}
}