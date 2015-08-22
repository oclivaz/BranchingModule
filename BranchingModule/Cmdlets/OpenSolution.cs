using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Open, "Solution")]
	public class OpenSolution : PSCmdlet, ITextOutputListener
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 1
			)]
		public string Branch { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IControllerFactory factory = new ControllerFactory();
			OpenSolutionController controller = factory.Get<OpenSolutionController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.OpenSolution(new BranchInfo(this.Teamproject, this.Branch ?? "Main"));
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