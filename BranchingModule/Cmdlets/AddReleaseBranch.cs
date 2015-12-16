using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Add, "Releasebranch")]
	public class AddReleasebranch : BranchingModulePSCmdletBase
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
			AddReleasebranchController controller = ControllerFactory.Get<AddReleasebranchController>();

			controller.AddReleasebranch(new BranchInfo(this.Teamproject, this.Branch));
		}
		#endregion
	}
}