using System;
using System.IO;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class DumpRepositoryService : IDumpRepositoryService
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		public IFileSystemService FileSystem { get; set; }
		private ISettings Settings { get; set; }
		public ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public DumpRepositoryService(ISourceControlService sourceControlService, IFileSystemService fileSystemService, ISettings settings, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(settings == null) throw new ArgumentNullException("settings");

			this.SourceControl = sourceControlService;
			this.FileSystem = fileSystemService;
			this.Settings = settings;
			this.TextOutput = textOutputService;
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