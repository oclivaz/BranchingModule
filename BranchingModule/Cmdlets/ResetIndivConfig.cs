using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Reset, "IndivConfig")]
	public class ResetIndivConfig : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public ResetIndivConfig()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			ResetIndivConfigController controller = ControllerFactory.Get<ResetIndivConfigController>();

			controller.ResetIndivConfig(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}