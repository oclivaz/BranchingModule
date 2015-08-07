using System;

namespace BranchingModule.Logic
{
	public struct BranchInfo
	{
		#region Properties
		public string TeamProject { get; private set; }
		public string Branch { get; private set; }
		public string AppTitle { get { return string.Format("{0} Release {1}", this.TeamProject, this.Branch); } }
		#endregion

		#region Constructors
		public BranchInfo(string teamProject, string branch) : this()
		{
			if(teamProject == null) throw new ArgumentNullException("teamProject");
			if(branch == null) throw new ArgumentNullException("branch");

			this.TeamProject = teamProject;
			this.Branch = branch;
		}
		#endregion
	}
}