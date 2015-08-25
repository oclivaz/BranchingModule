using System;
using System.Collections.Generic;

namespace BranchingModule.Logic
{
	public struct BranchInfo
	{
		#region Properties
		public string TeamProject { get; private set; }

		public string Name { get; private set; }
		#endregion

		#region Constructors
		public BranchInfo(string teamProject, string name) : this()
		{
			if(teamProject == null) throw new ArgumentNullException("teamProject");
			if(name == null) throw new ArgumentNullException("name");

			this.TeamProject = teamProject;
			this.Name = name;
		}
		#endregion

		#region Publics
		public static BranchInfo Create(string strTeamProject, string strBranch)
		{
			if(strTeamProject == null) throw new ArgumentNullException("strTeamProject");
			if(string.IsNullOrEmpty(strTeamProject)) throw new ArgumentException("No TeamProject provided", strTeamProject);

			if(!string.IsNullOrEmpty(strBranch)) return new BranchInfo(strTeamProject, strBranch);

			return new BranchInfo(strTeamProject, "Main");
		}

		public static ISet<BranchInfo> CreateSet(string strTeamProject, string[] names)
		{
			ISet<BranchInfo> set = new HashSet<BranchInfo>();
			Array.ForEach(names, name => set.Add(new BranchInfo(strTeamProject, name)));

			return set;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", this.TeamProject, this.Name);
		}
		#endregion
	}
}