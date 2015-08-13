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
		public void MergeBugfix(string strTeamproject, string strChangeset, string[] targetBranches, bool bNoCheckIn)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");
			if(targetBranches == null) throw new ArgumentNullException("targetBranches");

			BranchInfo sourceBranch = this.VersionControl.GetBranchInfoByChangeset(strChangeset);

			if(bNoCheckIn) MergeChangesetWithoutCheckIn(strTeamproject, strChangeset, targetBranches, sourceBranch);
			else MergeChangesetWithCheckIn(strTeamproject, strChangeset, targetBranches, sourceBranch);
		}

		private void MergeChangesetWithCheckIn(string strTeamproject, string strChangeset, string[] targetBranches, BranchInfo sourceBranch)
		{
			if(this.Convention.GetBranchType(sourceBranch) == BranchType.Release)
			{
				strChangeset = this.VersionControl.MergeChangeset(strChangeset, sourceBranch, this.Convention.MainBranch(strTeamproject));
				sourceBranch = this.Convention.MainBranch(strTeamproject);
			}

			this.VersionControl.MergeChangeset(strChangeset, sourceBranch, BranchInfo.CreateSet(strTeamproject, targetBranches));
		}

		private void MergeChangesetWithoutCheckIn(string strTeamproject, string strChangeset, string[] targetBranches, BranchInfo sourceBranch)
		{
			if(this.Convention.GetBranchType(sourceBranch) == BranchType.Release)
			{
				this.VersionControl.MergeChangesetWithoutCheckIn(strChangeset, sourceBranch, this.Convention.MainBranch(strTeamproject));
				return;
			}

			this.VersionControl.MergeChangesetWithoutCheckIn(strChangeset, sourceBranch, BranchInfo.CreateSet(strTeamproject, targetBranches));
		}
		#endregion
	}
}