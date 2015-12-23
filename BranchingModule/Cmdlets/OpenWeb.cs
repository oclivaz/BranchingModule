using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Open, "Web")]
	public class OpenWeb : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public OpenWeb()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			OpenWebController controller = ControllerFactory.Get<OpenWebController>();

			controller.OpenWeb(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}