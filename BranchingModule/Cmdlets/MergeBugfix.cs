using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet(VerbsData.Merge, "Bugfix")]
	public class MergeBugfix : PSCmdlet, ITextOutputListener, IUserInputProvider
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

		#region Publics
		public bool RequestConfirmation(string strMessageToConfirm)
		{
			Console.WriteLine(strMessageToConfirm);
			string strInput = Console.ReadLine();

			return strInput == "yes";
		}
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IControllerFactory factory = new ControllerFactory();
			MergeBugfixController controller = factory.Get<MergeBugfixController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			IUserInputService userInputService = factory.Get<IUserInputService>();
			userInputService.SetProvider(this);

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