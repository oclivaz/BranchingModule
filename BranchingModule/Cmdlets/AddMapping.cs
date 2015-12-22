using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Add, "Mapping")]
	public class AddMapping : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		internal DynamicParameter<SwitchParameter> Minimal { get; set; }
		internal DynamicParameter<SwitchParameter> OpenSolution { get; set; }
		#endregion

		#region Constructors
		public AddMapping()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
			this.Minimal = DynamicParameterFactory.CreateMinimalParameter(this, 2);
			this.OpenSolution = DynamicParameterFactory.CreateOpenSolutionParameter(this, 3);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			AddMappingController controller = ControllerFactory.Get<AddMappingController>();

			controller.AddMapping(BranchInfo.Create(this.Teamproject, this.Branch), this.Minimal.Value, this.OpenSolution.Value);
		}
		#endregion
	}
}