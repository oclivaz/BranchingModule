using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet("Merge", "Changeset")]
	public class MergeChangeset : PSCmdlet, ITextOutputListener
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
		public string Changeset { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 2
			)]
		public string Targetbranches { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();
			MergeChangesetController controller = factory.Get<MergeChangesetController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.MergeChangeset(this.Teamproject, this.Changeset, this.Targetbranches.Split(','));
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