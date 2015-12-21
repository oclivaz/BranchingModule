using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Remove, "Releasebranch")]
	public class RemoveReleasebranch : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public RemoveReleasebranch()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
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