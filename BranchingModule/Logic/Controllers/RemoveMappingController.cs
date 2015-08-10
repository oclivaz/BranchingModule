using System;

namespace BranchingModule.Logic
{
	internal class RemoveMappingController
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		public IAdeNetService AdeNet { get; set; }
		public IFileSystemService FileSystem { get; set; }
		public IConvention Convention { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveMappingController(ISourceControlService sourceControlService, IAdeNetService adeNetService, IFileSystemService fileSystemService, IConvention convention, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(fileSystemService == null) throw new ArgumentNullException("fileSystemService");
			if(convention == null) throw new ArgumentNullException("convention");

			this.SourceControl = sourceControlService;
			this.AdeNet = adeNetService;
			this.FileSystem = fileSystemService;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveMapping(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Mapping");
			this.SourceControl.DeleteMapping(branch);

			this.TextOutput.WriteVerbose("Deleting Solution");
			this.FileSystem.DeleteDirectory(this.Convention.GetLocalPath(branch));

			this.TextOutput.WriteVerbose("Removing Application");
			this.AdeNet.RemoveApplication(branch);
		}
		#endregion
	}
}