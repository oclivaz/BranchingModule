using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Restore, "Database")]
	public class RestoreDatabase : BranchingModulePSCmdletBase
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

		[Parameter(
			Mandatory = false,
			Position = 2
			)]
		public string File { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			RestoreDatabaseController controller = ControllerFactory.Get<RestoreDatabaseController>();

			if(string.IsNullOrEmpty(this.File)) controller.RestoreDatabase(BranchInfo.Create(this.Teamproject, this.Branch));
			else controller.RestoreDatabase(BranchInfo.Create(this.Teamproject, this.Branch), this.File);
		}
		#endregion
	}
}