using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Get, "Latest")]
	public class GetLatest : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		internal DynamicParameter<SwitchParameter> OpenSolution { get; set; }
		internal DynamicParameter<SwitchParameter> OpenWeb { get; set; }
		#endregion

		#region Constructors
		public GetLatest()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
			this.OpenSolution = DynamicParameterFactory.CreateOpenSolutionParameter(this, 2);
			this.OpenWeb = DynamicParameterFactory.CreateOpenWebParameter(this, 3);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			GetLatestController controller = ControllerFactory.Get<GetLatestController>();

			controller.GetLatest(BranchInfo.Create(this.Teamproject, this.Branch), this.OpenSolution.Value, this.OpenWeb.Value);
		}
		#endregion
	}
}