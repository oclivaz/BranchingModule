using System;
using System.Collections.Generic;

namespace BranchingModule.Logic
{
	internal class ShowReleasebranchesController
	{
		#region Properties
		public ITextOutputService TextOutput { get; set; }
		private IVersionControlService VersionControl { get; set; }
		#endregion

		#region Constructors
		public ShowReleasebranchesController(IVersionControlService versionControlService, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.VersionControl = versionControlService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void ShowReleasebranches(string strTeamproject)
		{
			ISet<BranchInfo> releasebranches = this.VersionControl.GetReleasebranches(strTeamproject);

			foreach(BranchInfo releasebranch in releasebranches)
			{
				this.TextOutput.Write(String.Format("{0}{1}", releasebranch, this.VersionControl.IsMapped(releasebranch) ? ", mapped" : string.Empty));
			}
		}
		#endregion
	}
}