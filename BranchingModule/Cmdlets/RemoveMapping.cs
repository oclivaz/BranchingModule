using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Remove, "Mapping")]
	public class RemoveMapping : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public RemoveMapping()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
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