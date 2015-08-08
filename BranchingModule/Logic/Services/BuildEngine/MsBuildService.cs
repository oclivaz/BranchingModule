using System;
using System.Diagnostics;

namespace BranchingModule.Logic
{
	internal class MsBuildService : IBuildEngineService
	{
		#region Properties
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MsBuildService(IConvention convention, ISettings settings)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void Build(BranchInfo branch)
		{
			string strArguments = string.Format(@"{0}\{1}.sln /t:Build", this.Convention.GetLocalPath(branch), branch.TeamProject);

			ProcessStartInfo startInfo = new ProcessStartInfo(this.Settings.MSBuildExePath, strArguments)
			{
				Arguments = strArguments,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			using(Process process = Process.Start(startInfo))
			{
				if(process != null) process.WaitForExit();
			}
		}
		#endregion
	}
}