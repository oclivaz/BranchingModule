using System;
using System.Diagnostics;

namespace BranchingModule.Logic
{
	internal class AdeNetAdapter : IAdeNetAdapter
	{
		#region Properties
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AdeNetAdapter(IConvention convention, ISettings settings)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void InstallPackages(BranchInfo branch)
		{
			string strArguments = string.Format(@"/C {0}\AdeNet.exe -workingdirectory {1} -deploy -development", this.Settings.AdeNetExePath, this.Convention.GetLocalPath(branch));

			ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
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

		public void BuildWebConfig(BranchInfo branch)
		{
			string strArguments = string.Format(@"/C {0}\AdeNet.exe -workingdirectory {1} -buildwebconfig -development", this.Settings.AdeNetExePath, this.Convention.GetLocalPath(branch));

			ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
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