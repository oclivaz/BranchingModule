using System;
using System.Collections.Generic;

namespace BranchingModule.Logic
{
	internal class GetReleasebranchesController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		public ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public GetReleasebranchesController(IVersionControlService versionControlService, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.VersionControl = versionControlService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void GetReleasebranches(string strTeamproject)
		{
			ISet<BranchInfo> releasebranches = this.VersionControl.GetReleasebranches(strTeamproject);

			foreach(BranchInfo releasebranch in releasebranches)
			{
				this.TextOutput.Write(releasebranch.ToString());
			}
		}
		#endregion
	}
}