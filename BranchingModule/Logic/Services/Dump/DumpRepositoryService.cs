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
		public IFileSystemService FileSystem { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public DumpRepositoryService(ISourceControlService sourceControlService, IFileSystemService fileSystemService, ISettings settings)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(settings == null) throw new ArgumentNullException("settings");

			this.SourceControl = sourceControlService;
			this.FileSystem = fileSystemService;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void CopyDump(BranchInfo branch, string strTarget)
		{
			DateTime dtBranchCreation = this.SourceControl.GetCreationTime(branch);

			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);

			var archivesBevoreCreation = from dumpArchive in this.FileSystem.GetFiles(this.Settings.DumpRepositoryPath)
										 where dumpArchive.FileName.StartsWith(teamProjectSettings.RefDB)
											   && dumpArchive.CreationTime < dtBranchCreation
										 select dumpArchive;

			IFileInfo newestArchive = archivesBevoreCreation.OrderByDescending(fileInfo => fileInfo.CreationTime).First();

			string strTargetDirectory = Path.GetDirectoryName(strTarget);
			if(strTargetDirectory == null) throw new Exception("Couldn't determine target directory");

			string strLocalArchive = String.Format(@"{0}\{1}", this.Settings.TempDirectory, newestArchive.FileName);
			this.FileSystem.Copy(newestArchive.FullName, strLocalArchive);

			this.FileSystem.ExtractZip(strLocalArchive, this.Settings.TempDirectory);

			this.FileSystem.Move(string.Format(@"{0}\{1}.bak", this.Settings.TempDirectory, teamProjectSettings.RefDB), strTarget);
			this.FileSystem.Delete(strLocalArchive);
		}
		#endregion
	}
}