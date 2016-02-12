using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.New, "Database")]
	public class NewDatabase : BranchingModulePSCmdletBase
	{
		#region Properties
		internal DynamicParameter<string> Teamproject { get; set; }
		internal DynamicParameter<string> Branch { get; set; }
		internal DynamicParameter<string> File { get; set; }
		#endregion

		#region Constructors
		public NewDatabase()
		{
			this.Teamproject = DynamicParameterFactory.CreateTeamProjectParameter(this, 0);
			this.Branch = DynamicParameterFactory.CreateBranchParameter(this, 1);
			this.File = new DynamicParameter<string>(this, "File", false, 2);
		}
		#endregion

		#region Protecteds
		protected override void OnProcessRecord()
		{
			NewDatabaseController controller = ControllerFactory.Get<NewDatabaseController>();

			controller.CreateDatabase(BranchInfo.Create(this.Teamproject, this.Branch));
		}
		#endregion
	}
}