﻿using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace BranchingModule.Logic
{
	internal class DumpRepositoryService : IDumpRepositoryService
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public DumpRepositoryService(ISourceControlService sourceControlService, ISettings settings)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(settings == null) throw new ArgumentNullException("settings");

			this.SourceControl = sourceControlService;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void CopyDump(BranchInfo branch, string strTarget)
		{
			DateTime dtBranchCreation = this.SourceControl.GetCreationTime(branch);

			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);

			var archivesBevoreCreation = from dumpArchive in Directory.GetFiles(this.Settings.DumpRepositoryPath)
			                             let fileInfo = new FileInfo(dumpArchive)
			                             where fileInfo.Name.StartsWith(teamProjectSettings.RefDB)
			                                   && fileInfo.CreationTime < dtBranchCreation
			                             select fileInfo;

			FileInfo newestArchive = archivesBevoreCreation.OrderByDescending(fileInfo => fileInfo.CreationTime).First();

			string strTargetDirectory = Path.GetDirectoryName(strTarget);
			if(strTargetDirectory == null) throw new Exception("Couldn't determine target directory");

			string strLocalArchive = String.Format(@"{0}\{1}",strTargetDirectory, newestArchive.Name);
			File.Copy(newestArchive.FullName, strLocalArchive, true);

			FastZip fastZip = new FastZip();
			fastZip.ExtractZip(strLocalArchive, this.Settings.TempDirectory, null);

			File.Move(string.Format(@"{0}\{1}.bak", this.Settings.TempDirectory, teamProjectSettings.RefDB), strTarget);
			File.Delete(strLocalArchive);
		}
		#endregion
	}
}