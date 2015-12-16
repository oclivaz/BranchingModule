using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Get, "Releasebranches")]
	public class GetReleasebranches : BranchingModulePSCmdletBase
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			GetReleasebranchesController controller = ControllerFactory.Get<GetReleasebranchesController>();

			controller.GetReleasebranches(this.Teamproject);
		}
		#endregion
	}
}