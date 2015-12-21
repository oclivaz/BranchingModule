using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Open, "Solution")]
	public class OpenSolution : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		#endregion

		#region Constructors
		public OpenSolution()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			OpenSolutionController controller = ControllerFactory.Get<OpenSolutionController>();

			controller.OpenSolution(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}