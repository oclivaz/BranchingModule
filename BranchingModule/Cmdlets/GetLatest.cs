using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Get, "Latest")]
	public class GetLatest : PSCmdlet, ITextOutputListener
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

		[Parameter(
			Mandatory = false,
			Position = 3
			)]
		public SwitchParameter OpenSolution { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IControllerFactory factory = new ControllerFactory();
			GetLatestController controller = factory.Get<GetLatestController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.GetLatest(BranchInfo.Create(this.Teamproject, this.Branch), this.OpenSolution);
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