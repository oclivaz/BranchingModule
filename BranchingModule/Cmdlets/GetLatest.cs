using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Get, "Latest")]
	public class GetLatest : BranchingModulePSCmdletBase
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
			Position = 3
			)]
		public SwitchParameter OpenSolution { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			GetLatestController controller = ControllerFactory.Get<GetLatestController>();

			controller.GetLatest(BranchInfo.Create(this.Teamproject, this.Branch), this.OpenSolution);
		}
		#endregion
	}
}