using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsCommon.Remove, "Releasebranch")]
	public class RemoveReleasebranch : PSCmdlet, ITextOutputListener
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
			IControllerFactory factory = new ControllerFactory();
			RemoveReleasebranchController controller = factory.Get<RemoveReleasebranchController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.RemoveReleasebranch(new BranchInfo(this.Teamproject, this.Branch));
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