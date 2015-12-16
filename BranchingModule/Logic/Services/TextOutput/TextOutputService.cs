using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BranchingModule.Logic
{
	internal class TextOutputService : ITextOutputService
	{
		#region Constants
		private const string UNKNOWN = "unknown";
		#endregion

		#region Properties
		private ISet<ITextOutputListener> Listeners { get; set; }
		#endregion

		#region Constructors
		public TextOutputService()
		{
			this.Listeners = new HashSet<ITextOutputListener>();
		}
		#endregion

		#region Publics
		public void RegisterListener(ITextOutputListener listener)
		{
			this.Listeners.Add(listener);
		}

		public void WriteVerbose(string strText)
		{
			if(strText == null) throw new ArgumentNullException("strText");

			StackTrace stackTrace = new StackTrace();
			StackFrame stackFrame = stackTrace.GetFrame(1);
			MethodBase sourceMethod = stackFrame.GetMethod();

			string sourceClassName = sourceMethod.ReflectedType == null ? UNKNOWN : sourceMethod.ReflectedType.Name;

			foreach(var listener in this.Listeners)
			{
				listener.WriteVerbose(string.Format("{0}.{1}: {2}", sourceClassName, sourceMethod.Name, strText));
			}
		}

		public void Write(string strText)
		{
			if(strText == null) throw new ArgumentNullException("strText");

			foreach(var listener in this.Listeners)
			{
				listener.Write(strText);
			}
		}
		#endregion
	}
}