using System;

namespace BranchingModule.Logic
{
	internal class RemoveReleasebranchController
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		public IFileSystemService FileSystem { get; set; }
		public IConvention Convention { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveReleasebranchController(ISourceControlService sourceControlService, IFileSystemService fileSystemService, IConvention convention, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.SourceControl = sourceControlService;
			this.FileSystem = fileSystemService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveReleasebranch(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Mapping (if any)");
			this.SourceControl.DeleteMapping(branch);
			
			this.TextOutput.WriteVerbose("Deleting Branch");
			this.SourceControl.DeleteBranch(branch);

			this.TextOutput.WriteVerbose("Deleting buildserver dump");
			this.FileSystem.DeleteFile(this.Convention.GetBuildserverDump(branch));
		}
		#endregion
	}
}