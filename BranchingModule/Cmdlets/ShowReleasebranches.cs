using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Show, "Releasebranches")]
	public class ShowReleasebranches : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		#endregion

		#region Constructors
		public ShowReleasebranches()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			ShowReleasebranchesController controller = ControllerFactory.Get<ShowReleasebranchesController>();

			controller.ShowReleasebranches(this.Teamproject);
		}
		#endregion
	}
}