using System;
using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class MergeBugfixController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IUserInputService UserInputService { get; set; }
		private ISettings Settings { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public MergeBugfixController(IVersionControlService versionControlService, IUserInputService userInputService, ISettings settings, IConvention convention)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControl = versionControlService;
			this.UserInputService = userInputService;
			this.Settings = settings;
			this.Convention = convention;
		}
		#endregion

		#region Publics
		public void MergeBugfix(string strChangeset, string[] targetBranches)
		{
			if(strChangeset == null) throw new ArgumentNullException("strChangeset");
			if(targetBranches == null) throw new ArgumentNullException("targetBranches");

			string strComment = this.VersionControl.GetChangesetComment(strChangeset);
			if(!this.UserInputService.RequestConfirmation(string.Format("Merge Changeset {0} - \"{1}\"?", strChangeset, strComment)))
			{
				return;
			}

			BranchInfo sourceBranch = this.VersionControl.GetBranchInfoByChangeset(strChangeset);

			if(!this.Settings.IsSupportedTeamproject(sourceBranch.TeamProject))
			{
				throw new NotSupportedException(string.Format("The teamproject {0} is not supported", sourceBranch.TeamProject));
			}

			if(!targetBranches.Any()) targetBranches = GetTargetBranches(sourceBranch.TeamProject, sourceBranch);

			MergeChangeset(strChangeset, sourceBranch, targetBranches);
		}
		#endregion

		#region Privates
		private string[] GetTargetBranches(string strTeamproject, BranchInfo sourceBranch)
		{
			ISet<BranchInfo> allReleaseBranches = this.VersionControl.GetReleasebranches(strTeamproject);
			allReleaseBranches.ExceptWith(new[] { sourceBranch });
			return allReleaseBranches.Select(branch => branch.Name).ToArray();
		}

		private void MergeChangeset(string strChangeset, BranchInfo sourceBranch, string[] targetBranches)
		{
			if(this.Convention.GetBranchType(sourceBranch) == BranchType.Release)
			{
				strChangeset = this.VersionControl.MergeChangeset(strChangeset, sourceBranch, this.Convention.MainBranch(sourceBranch.TeamProject));
				sourceBranch = this.Convention.MainBranch(sourceBranch.TeamProject);
			}

			this.VersionControl.MergeChangeset(strChangeset, sourceBranch, BranchInfo.CreateSet(sourceBranch.TeamProject, targetBranches));
		}

		#endregion
	}
}