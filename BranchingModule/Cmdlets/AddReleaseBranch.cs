using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Add, "Releasebranch")]
	public class AddReleasebranch : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public AddReleasebranch()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
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