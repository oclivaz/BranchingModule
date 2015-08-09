namespace BranchingModule.Logic
{
	internal class MSConvention : IConvention
	{
		#region Properties
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MSConvention(ISettings settings)
		{
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public string GetLocalPath(BranchInfo branch)
		{
			return string.Format(@"C:\inetpub\wwwroot\{0}_{1}", branch.TeamProject, branch.Name.Replace('.', '_'));
		}

		public string GetServerPath(BranchInfo branch)
		{
			if(branch.Name == BranchInfo.MAIN) return string.Format(@"$/{0}/Main/Source", branch.TeamProject);
			return string.Format(@"$/{0}/Release/{1}/Source", branch.TeamProject, branch.Name);
		}

		public string GetBuildserverDump(BranchInfo branch)
		{
			return string.Format(@"\\build\Backup\{0}_Release_{1}.bak", branch.TeamProject, branch.Name);
		}

		public string GetLocalDump(BranchInfo branch)
		{
			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);
			return string.Format(@"c:\Database\{0}\{1}_Release_{2}.bak", teamProjectSettings.LocalDB, branch.TeamProject, branch.Name);
		}
		#endregion
	}
}