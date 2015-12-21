using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Merge, "Bugfix")]
	public class MergeBugfix : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Changeset { get; set; }
		internal DynamicParameter<string> Targetbranches { get; set; }
		#endregion

		#region Constructors
		public MergeBugfix()
		{
			this.Changeset = new DynamicParameter<string>(this, "Changeset", true, 0);
			this.Targetbranches = new DynamicParameter<string>(this, "Targetbranches", false, 1);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			MergeBugfixController controller = ControllerFactory.Get<MergeBugfixController>();

			string[] targetBranches = this.Targetbranches != null ? this.Targetbranches.ToString().Split(',') : new string[0];
			controller.MergeBugfix(this.Changeset, targetBranches);
		}
		#endregion
	}
}