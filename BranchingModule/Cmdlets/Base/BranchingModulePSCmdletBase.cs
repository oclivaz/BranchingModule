using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	public abstract class BranchingModulePSCmdletBase : PSCmdlet, ITextOutputListener, IUserInputProvider, IDynamicParameters
	{
		#region Fields
		private readonly IList<IDynamicParameter> DynamicParameters;
		#endregion

		#region Constructors
		protected BranchingModulePSCmdletBase()
		{
			this.DynamicParameters = new List<IDynamicParameter>();
		}
		#endregion

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

		public object GetDynamicParameters()
		{
			RuntimeDefinedParameterDictionary parameters = new RuntimeDefinedParameterDictionary();

			foreach(IDynamicParameter dynamicParameter in this.DynamicParameters)
			{
				dynamicParameter.AddRuntimeDefinedParameterTo(parameters);
			}

			return parameters;
		}
		#endregion

		#region Internals
		internal void AddDynamicParameter(IDynamicParameter dynamicParameter)
		{
			this.DynamicParameters.Add(dynamicParameter);
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

				WriteVerbose(GetElapsedTime(watch));
			}
			catch(AggregateException ex)
			{
				foreach(Exception innerException in ex.InnerExceptions)
				{
					Console.WriteLine(innerException.Message);
				}
				
				Console.WriteLine(ex.StackTrace);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				textOutputService.UnregisterListener(this);
			}
		}

		protected abstract void OnProcessRecord();
		#endregion

		#region Privates
		private static string GetElapsedTime(Stopwatch watch)
		{
			if(watch == null) throw new ArgumentNullException("watch");

			StringBuilder builder = new StringBuilder();
			builder.Append("Finished in ");
			if(watch.Elapsed.Minutes > 0) builder.AppendFormat("{0} minutes and ", watch.Elapsed.Minutes);
			builder.AppendFormat("{0} seconds.", watch.Elapsed.Seconds
			                                     + (decimal) watch.Elapsed.Milliseconds / 1000);

			return builder.ToString();
		}
		#endregion
	}
}