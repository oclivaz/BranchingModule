using System;

namespace BranchingModule.Logic
{
	internal class MergeBugfixController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public MergeBugfixController(IVersionControlService versionControlService, IConvention convention)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControl = versionControlService;
			this.Convention = convention;
		}
		#endregion

		#region Publics
		public void MergeBugfix(string strTeamproject, string strChangeset, string[] targetBranches)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");
			if(targetBranches == null) throw new ArgumentNullException("targetBranches");

			BranchInfo sourceBranch = this.VersionControl.GetBranchInfo(strChangeset);

			if(this.Convention.IsReleasebranch(sourceBranch))
			{
				strChangeset = this.VersionControl.MergeChangeset(strChangeset, sourceBranch, this.Convention.MainBranch(strTeamproject));
				sourceBranch = this.Convention.MainBranch(strTeamproject);
			}

			this.VersionControl.MergeChangeset(strChangeset, sourceBranch, BranchInfo.CreateSet(strTeamproject, targetBranches));
		}
		#endregion
	}
}