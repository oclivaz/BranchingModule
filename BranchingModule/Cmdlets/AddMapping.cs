using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets

{
	[Cmdlet(VerbsCommon.Add, "Mapping")]
	public class AddMapping : PSCmdlet, ITextOutputListener
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

		[Parameter(
			Mandatory = false,
			Position = 2
			)]
		public SwitchParameter Minimal { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();
			AddMappingController controller = factory.Get<AddMappingController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.AddMapping(new BranchInfo(this.Teamproject, this.Branch), this.Minimal);
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