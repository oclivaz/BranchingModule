﻿using System;

namespace BranchingModule.Logic
{
	public struct BranchInfo
	{
		#region Constants
		public const string MAIN = "main";
		#endregion

		#region Properties
		public string TeamProject { get; private set; }
		public string Name { get; private set; }

		public string AppTitle
		{
			get { return string.Format("{0} Release {1}", this.TeamProject, this.Name); }
		}
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
		public static BranchInfo Main(string teamProject)
		{
			return new BranchInfo(teamProject, MAIN);
		}
		#endregion
	}
}