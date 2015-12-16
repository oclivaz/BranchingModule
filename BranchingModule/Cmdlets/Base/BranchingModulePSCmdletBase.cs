using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	public abstract class BranchingModulePSCmdletBase : PSCmdlet, ITextOutputListener, IUserInputProvider
	{
		#region Publics
		public void Write(string strText)
		{
			WriteObject(strText);
		}

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
			ITextOutputService textOutputService = ControllerFactory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			IUserInputService userInputService = ControllerFactory.Get<IUserInputService>();
			userInputService.SetProvider(this);

			try
			{
				OnProcessRecord();
			}
			catch(Exception ex)
			{
				WriteObject(ex.StackTrace);
				throw;
			}
		}

		protected abstract void OnProcessRecord();
		#endregion
	}
}