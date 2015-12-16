using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Remove, "Releasebranch")]
	public class RemoveReleasebranch : BranchingModulePSCmdletBase
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }

		[Parameter(
			Mandatory = true,
			Position = 1
			)]
		public string Branch { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			RemoveReleasebranchController controller = ControllerFactory.Get<RemoveReleasebranchController>();

			controller.RemoveReleasebranch(new BranchInfo(this.Teamproject, this.Branch));
		}
		#endregion
	}
}