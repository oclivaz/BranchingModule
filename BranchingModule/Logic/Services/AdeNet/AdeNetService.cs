using System;
using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class AdeNetService : IAdeNetService
	{
		#region Properties
		private ITextOutputService TextOutput { get; set; }
		private IFileExecutionService FileExecution { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public AdeNetService(IFileExecutionService fileExecutionService, IConvention convention, ISettings settings, ITextOutputService textOutputService)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.FileExecution = fileExecutionService;
			this.Convention = convention;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void InstallPackages(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -deploy -development", this.Convention.GetLocalPath(branch)));

			foreach(string strReference in this.Settings.GetTeamProjectSettings(branch.TeamProject).AditionalReferences)
			{
				InstallLatestPackage(branch, strReference);
			}
		}

		public void BuildWebConfig(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -buildwebconfig -development", this.Convention.GetLocalPath(branch)));
		}

		public void InitializeIIS(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-initializeiis {0}", this.Convention.GetLocalPath(branch)));
		}

		public void CleanupIIS(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-cleanupiis {0}", this.Convention.GetLocalPath(branch)));
		}

		public void CreateBuildDefinition(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose(string.Format("Starting Internet Explorer. Add a build configuration for {0}", branch));
			this.FileExecution.StartProcess(Executables.INTERNET_EXPLORER, this.Settings.BuildConfigurationUrl);
		}

		public void CreateDatabase(BranchInfo branch)
		{
			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-createdb {0}", this.Convention.GetLocalPath(branch)));
		}
		#endregion

		#region Privates
		private void InstallLatestPackage(BranchInfo branch, string strReference)
		{
			string[] packages = ListPackages(strReference);

			string package = FindLastRevisionOrBugfix(packages);

			if(package != null)
			{
				InstallPackage(branch, package);
			}
		}

		private void InstallPackage(BranchInfo branch, string package)
		{
			string strPackage = package.Replace(":", string.Empty);
			this.TextOutput.WriteVerbose(string.Format("Installing {0}", strPackage));

			this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-workingdirectory {0} -install {1} -development", this.Convention.GetLocalPath(branch), strPackage));
		}

		private static string FindLastRevisionOrBugfix(IEnumerable<string> packages)
		{
			return packages.LastOrDefault(p => !string.IsNullOrEmpty(p) && !p.Contains("-"));
		}

		private string[] ListPackages(string strReference)
		{
			return this.FileExecution.ExecuteInCmd(this.Settings.AdeNetExePath, string.Format("-list {0}", strReference)).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
		}
		#endregion
	}
}