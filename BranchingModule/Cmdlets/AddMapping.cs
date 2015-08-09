using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets

{
	[Cmdlet(VerbsCommon.Add, "Mapping")]
	public class AddMapping : PSCmdlet
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }

		[Parameter(
			Mandatory = true,
			Position = 1
			)]
		public string Branch { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IObjectFactory factory = new ObjectFactory();
			AddMappingController controller = factory.Get<AddMappingController>();

			try
			{
				controller.Process(new BranchInfo(this.Teamproject, this.Branch));
			}
			catch(Exception ex)
			{
				WriteObject(ex.StackTrace);
				throw;
			}
		}
		#endregion
	}
}