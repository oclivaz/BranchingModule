using System;
using System.Linq;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	public class ConventionDummy : IConvention
	{
		#region Properties
		private IBranchConventionFactory BranchConventionFactory { get; set; }
		#endregion

		#region Constructors
		public ConventionDummy()
		{
			this.BranchConventionFactory = new BranchConventionFactory();
			this.BranchConventionFactory.RegisterBranchConvention(new MainbranchConventionDummy());
			this.BranchConventionFactory.RegisterBranchConvention(new ReleasebranchConventionDummy());
		}
		#endregion

		#region Publics
		public static BranchInfo MainBranch(string teamProject)
		{
			return new BranchInfo(teamProject, MainbranchConventionDummy.MAIN);
		}

		BranchInfo IConvention.MainBranch(string teamProject)
		{
			return MainBranch(teamProject);
		}

		public BranchInfo GetBranchInfoByServerPath(string strServerPath)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public BranchType GetBranchType(BranchInfo branch)
		{
			var followedConvention = (from convention in this.BranchConventionFactory.GetAllConventions()
			                          where convention.BranchnameFollowsConvention(branch.Name)
			                          select convention).Single();

			return followedConvention.BranchType;
		}

		public string GetLocalPath(BranchInfo branch)
		{
			return Convention(branch).GetLocalPath(branch);
		}

		public string GetServerPath(BranchInfo branch)
		{
			return Convention(branch).GetServerPath(branch);
		}

		public string GetServerBasePath(BranchInfo branch)
		{
			return Convention(branch).GetServerBasePath(branch);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			return Convention(branch).GetBuildserverDump(branch);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			return Convention(branch).GetLocalDump(branch);
		}

		public string GetApplicationName(BranchInfo branch)
		{
			return Convention(branch).GetApplicationName(branch);
		}

		public string GetSolutionFile(BranchInfo branch)
		{
			return Convention(branch).GetSolutionFile(branch);
		}
		#endregion

		#region Privates
		private IBranchConvention Convention(BranchInfo branch)
		{
			return this.BranchConventionFactory.GetConvention(branch);
		}
		#endregion
	}
}
