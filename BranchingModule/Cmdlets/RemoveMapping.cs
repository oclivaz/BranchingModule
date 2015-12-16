using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Remove, "Mapping")]
	public class RemoveMapping : BranchingModulePSCmdletBase
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
			RemoveMappingController controller = ControllerFactory.Get<RemoveMappingController>();

			controller.RemoveMapping(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}