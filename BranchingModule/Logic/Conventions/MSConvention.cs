using System;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class MSConvention : IConvention
	{
		#region Properties
		private ISettings Settings { get; set; }
		private IBranchConventionRegistry BranchConventionRegistry { get; set; }
		#endregion

		#region Constructors
		public MSConvention(ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");

			this.Settings = settings;

			this.BranchConventionRegistry = new BranchConventionRegistry();
			this.BranchConventionRegistry.Register(new MSMainbranchConvention(this.Settings));
			this.BranchConventionRegistry.Register(new MSReleasebranchConvention(this.Settings));
			this.BranchConventionRegistry.Register(new MSDevelopmentBranchConvention(this.Settings));
		}
		#endregion

		#region Publics
		public BranchInfo MainBranch(string strTeamProject)
		{
			return new BranchInfo(strTeamProject, MSMainbranchConvention.MAIN);
		}

		public BranchInfo GetBranchInfoByServerPath(string strServerPath)
		{
			BranchInfo branch;
			bool bSucess = TryGetBranchInfoByServerPath(strServerPath, out branch);

			if(!bSucess) throw new Exception(string.Format("The serverpath {0} doesn't follow any known convention", strServerPath));

			return branch;
		}

		public bool TryGetBranchInfoByServerPath(string strServerPath, out BranchInfo branch)
		{
			var followedConvention = (from convention in this.BranchConventionRegistry.GetAllConventions()
									  where convention.ServerPathFollowsConvention(strServerPath)
									  select convention).ToArray();

			if(followedConvention.Count() > 1) throw new Exception(string.Format("The serverpath {0} follows multiple branch conventions", strServerPath));

			if(followedConvention.Any())
			{
				branch = followedConvention.Single().CreateBranchInfoByServerPath(strServerPath);
				return true;
			}

			branch = new BranchInfo();
			return false;
		}

		public BranchType GetBranchType(BranchInfo branch)
		{
			var followedConvention = (from convention in this.BranchConventionRegistry.GetAllConventions()
			                          where convention.BranchnameFollowsConvention(branch.Name)
			                          select convention).Single();

			return followedConvention.BranchType;
		}

		public string GetReleaseBranchesPath(string strTeamProject)
		{
			return string.Format(@"$/{0}/{1}", strTeamProject, MSReleasebranchConvention.RELEASE);
		}

		public string GetLocalPath(BranchInfo branch)
		{
			return Convention(branch).GetLocalPath(branch);
		}

		public string GetLocalDatabase(BranchInfo branch)
		{
			throw new NotImplementedException();
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