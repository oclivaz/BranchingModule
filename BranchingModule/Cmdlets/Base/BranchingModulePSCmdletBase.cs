using System;
using System.Diagnostics;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	public abstract class BranchingModulePSCmdletBase : PSCmdlet, ITextOutputListener, IUserInputProvider
	{
		#region Publics
		public void WriteLine(string strText)
		{
			Console.WriteLine(strText);
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
				Stopwatch watch = Stopwatch.StartNew();

				OnProcessRecord();

				watch.Stop();
				WriteVerbose(string.Format("Finished in {0} minutes, {1} seconds and {2} milliseconds.", watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.Milliseconds));
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
				throw;
			}
			finally
			{
				textOutputService.UnregisterListener(this);
			}
		}

		protected abstract void OnProcessRecord();
		#endregion
	}
}