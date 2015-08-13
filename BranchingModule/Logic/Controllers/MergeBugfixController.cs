﻿using System;
using System.Collections.Generic;
using System.Linq;

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
		public void MergeBugfix(string strChangeset, string[] targetBranches, bool bNoCheckIn)
		{
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");
			if(targetBranches == null) throw new ArgumentNullException("targetBranches");

			BranchInfo sourceBranch = this.VersionControl.GetBranchInfoByChangeset(strChangeset);

			if(!targetBranches.Any()) targetBranches = GetTargetBranches(sourceBranch.TeamProject, sourceBranch);

			if(bNoCheckIn) MergeChangesetWithoutCheckIn(strChangeset, sourceBranch, targetBranches);
			else MergeChangesetWithCheckIn(strChangeset, sourceBranch, targetBranches);
		}
		#endregion

		#region Privates
		private string[] GetTargetBranches(string strTeamproject, BranchInfo sourceBranch)
		{
			ISet<BranchInfo> allReleaseBranches = this.VersionControl.GetReleasebranches(strTeamproject);
			allReleaseBranches.ExceptWith(new[] { sourceBranch });
			return allReleaseBranches.Select(branch => branch.Name).ToArray();
		}

		private void MergeChangesetWithCheckIn(string strChangeset, BranchInfo sourceBranch, string[] targetBranches)
		{
			if(this.Convention.GetBranchType(sourceBranch) == BranchType.Release)
			{
				strChangeset = this.VersionControl.MergeChangeset(strChangeset, sourceBranch, this.Convention.MainBranch(sourceBranch.TeamProject));
				sourceBranch = this.Convention.MainBranch(sourceBranch.TeamProject);
			}

			this.VersionControl.MergeChangeset(strChangeset, sourceBranch, BranchInfo.CreateSet(sourceBranch.TeamProject, targetBranches));
		}

		private void MergeChangesetWithoutCheckIn(string strChangeset, BranchInfo sourceBranch, string[] targetBranches)
		{
			if(this.Convention.GetBranchType(sourceBranch) == BranchType.Release)
			{
				this.VersionControl.MergeChangesetWithoutCheckIn(strChangeset, sourceBranch, this.Convention.MainBranch(sourceBranch.TeamProject));
				return;
			}

			this.VersionControl.MergeChangesetWithoutCheckIn(strChangeset, sourceBranch, BranchInfo.CreateSet(sourceBranch.TeamProject, targetBranches));
		}
		#endregion
	}
}