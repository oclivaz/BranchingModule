using System;
using System.IO;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class DumpRepositoryService : IDumpRepositoryService
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		private ISettings Settings { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public DumpRepositoryService(IVersionControlService versionControlService, IFileSystemAdapter fileSystemAdapter, ISettings settings, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(settings == null) throw new ArgumentNullException("settings");

			this.VersionControl = versionControlService;
			this.FileSystem = fileSystemAdapter;
			this.Settings = settings;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void CopyDump(BranchInfo branch, string strTarget)
		{
			DateTime dtBranchCreation = this.VersionControl.GetCreationTime(branch);

			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);

			var archivesBevoreCreation = (from dumpArchive in this.FileSystem.GetFiles(this.Settings.DumpRepositoryPath)
										 where dumpArchive.FileName.StartsWith(teamProjectSettings.RefDB)
											   && dumpArchive.CreationTime < dtBranchCreation
										 select dumpArchive).ToArray();

			if(!archivesBevoreCreation.Any()) throw new Exception(string.Format("No dump of database {0} before {1} in Repository at {2}", teamProjectSettings.RefDB, dtBranchCreation, Settings.DumpRepositoryPath));
			IFileInfo newestArchive = archivesBevoreCreation.OrderByDescending(fileInfo => fileInfo.CreationTime).First();

			this.TextOutput.WriteVerbose(string.Format("Choosing {0} from Repository", newestArchive.FileName));

			string strTargetDirectory = Path.GetDirectoryName(strTarget);
			if(strTargetDirectory == null) throw new Exception("Couldn't determine target directory");

			string strLocalArchive = String.Format(@"{0}\{1}", this.Settings.TempDirectory, newestArchive.FileName);
			this.FileSystem.Copy(newestArchive.FullName, strLocalArchive);

			this.FileSystem.ExtractZip(strLocalArchive, this.Settings.TempDirectory);

			this.FileSystem.Move(string.Format(@"{0}\{1}.bak", this.Settings.TempDirectory, teamProjectSettings.RefDB), strTarget);
			this.FileSystem.DeleteFile(strLocalArchive);
		}
		#endregion
	}
}