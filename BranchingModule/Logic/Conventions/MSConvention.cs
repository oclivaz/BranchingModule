namespace BranchingModule.Logic
{
	internal class MSConvention : IConvention
	{
		public string GetLocalPath(BranchInfo branch)
		{
			return string.Format(@"C:\inetpub\wwwroot\{0}_{1}", branch.TeamProject, branch.Branch.Replace('.', '_'));
		}

		public string GetServerPath(BranchInfo branch)
		{
			return string.Format(@"$/{0}/Release/{1}/Source", branch.TeamProject, branch.Branch);
		}
	}
}
