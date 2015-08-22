using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Merge, "Bugfix")]
	public class MergeBugfix : PSCmdlet, ITextOutputListener
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Changeset { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 1
			)]
		public string Targetbranches { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();
			MergeBugfixController controller = factory.Get<MergeBugfixController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				string[] targetBranches = this.Targetbranches != null ? this.Targetbranches.Split(',') : new string[0];
				controller.MergeBugfix(this.Changeset, targetBranches);
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