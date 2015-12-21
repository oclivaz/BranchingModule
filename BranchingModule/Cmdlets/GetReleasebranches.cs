using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Get, "Releasebranches")]
	public class GetReleasebranches : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		#endregion

		#region Constructors
		public GetReleasebranches()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
		}
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