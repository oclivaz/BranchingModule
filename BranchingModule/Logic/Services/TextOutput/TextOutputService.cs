using System.Collections.Generic;

namespace BranchingModule.Logic
{
	internal class TextOutputService : ITextOutputService
	{
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
			foreach(var listener in this.Listeners)
			{
				listener.WriteVerbose(strText);
			}
		}
		#endregion
	}
}