using System;

namespace BranchingModule.Logic
{
	internal class RemoveReleasebranchController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		public IFileSystemAdapter FileSystem { get; set; }
		public IConvention Convention { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveReleasebranchController(IVersionControlService versionControlService, IFileSystemAdapter fileSystemAdapter, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.VersionControl = versionControlService;
			this.FileSystem = fileSystemAdapter;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveReleasebranch(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Mapping (if any)");
			this.VersionControl.DeleteMapping(branch);
			
			this.TextOutput.WriteVerbose("Deleting Branch");
			this.VersionControl.DeleteBranch(branch);

			this.TextOutput.WriteVerbose("Deleting buildserver dump");
			this.FileSystem.DeleteFile(this.Convention.GetBuildserverDump(branch));
		}
		#endregion
	}
}