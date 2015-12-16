using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Add, "Mapping")]
	public class AddMapping : BranchingModulePSCmdletBase
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
		public SwitchParameter Minimal { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 3
			)]
		public SwitchParameter OpenSolution { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			AddMappingController controller = ControllerFactory.Get<AddMappingController>();

			controller.AddMapping(BranchInfo.Create(this.Teamproject, this.Branch), this.Minimal, this.OpenSolution);
		}
		#endregion
	}
}