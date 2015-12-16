using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Backup, "Database")]
	public class BackupDatabase : BranchingModulePSCmdletBase
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 1
			)]
		public string Branch { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			BackupDatabaseController controller = ControllerFactory.Get<BackupDatabaseController>();

			controller.BackupDatabase(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}