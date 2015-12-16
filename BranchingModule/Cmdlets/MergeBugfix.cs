using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Merge, "Bugfix")]
	public class MergeBugfix : BranchingModulePSCmdletBase
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Changeset { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 1
			)]
		public string Targetbranches { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			MergeBugfixController controller = ControllerFactory.Get<MergeBugfixController>();

			string[] targetBranches = this.Targetbranches != null ? this.Targetbranches.Split(',') : new string[0];
			controller.MergeBugfix(this.Changeset, targetBranches);
		}
		#endregion
	}
}