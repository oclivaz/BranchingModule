using System;
using System.Linq;
using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	public class ConventionDummy : IConvention
	{
		#region Properties
		private IBranchConventionRegistry BranchConventionRegistry { get; set; }
		#endregion

		#region Constructors
		public ConventionDummy()
		{
			this.BranchConventionRegistry = new BranchConventionRegistry();
			this.BranchConventionRegistry.Register(new MainbranchConventionDummy());
			this.BranchConventionRegistry.Register(new ReleasebranchConventionDummy());
		}
		#endregion

		#region Publics
		public static BranchInfo MainBranch(string strTeamProject)
		{
			return new BranchInfo(strTeamProject, MainbranchConventionDummy.MAIN);
		}

		BranchInfo IConvention.MainBranch(string strTeamProject)
		{
			return MainBranch(strTeamProject);
		}

		public BranchInfo GetBranchInfoByServerPath(string strServerPath)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public bool TryGetBranchInfoByServerPath(string strServerPath, out BranchInfo branch)
		{
			throw new NotSupportedException("Create a mock please");
		}

		public BranchType GetBranchType(BranchInfo branch)
		{
			var followedConvention = (from convention in this.BranchConventionRegistry.GetAllConventions()
			                          where convention.BranchnameFollowsConvention(branch.Name)
			                          select convention).Single();

			return followedConvention.BranchType;
		}

		string IConvention.GetReleaseBranchesPath(string strTeamProject)
		{
			return GetReleaseBranchesPath(strTeamProject);
		}

		public static string GetReleaseBranchesPath(string strTeamProject)
		{
			return string.Format(@"ConventionDummy.GetReleaseBranchesPath {0}", strTeamProject);
		}

		public string GetLocalPath(BranchInfo branch)
		{
			return Convention(branch).GetLocalPath(branch);
		}

		public string GetServerPath(BranchInfo branch)
		{
			return Convention(branch).GetServerSourcePath(branch);
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

		public string GetAblagePath(BranchInfo branch)
		{
			return Convention(branch).GetAblagePath(branch);
		}
		#endregion

		#region Privates
		private IBranchConvention Convention(BranchInfo branch)
		{
			return this.BranchConventionRegistry.GetConvention(branch);
		}
		#endregion
	}
}
