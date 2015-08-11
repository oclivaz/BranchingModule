using System;

namespace BranchingModule.Logic
{
	internal class RemoveMappingController
	{
		#region Properties
		private IVersionControlService VersionControl { get; set; }
		public IAdeNetService AdeNet { get; set; }
		public IFileSystemAdapter FileSystem { get; set; }
		public IConvention Convention { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveMappingController(IVersionControlService versionControlService, IAdeNetService adeNetService, IFileSystemAdapter fileSystemAdapter, IConvention convention, ITextOutputService textOutputService)
		{
			if(versionControlService == null) throw new ArgumentNullException("versionControlService");
			if(adeNetService == null) throw new ArgumentNullException("adeNetService");
			if(fileSystemAdapter == null) throw new ArgumentNullException("fileSystemAdapter");
			if(convention == null) throw new ArgumentNullException("convention");

			this.VersionControl = versionControlService;
			this.AdeNet = adeNetService;
			this.FileSystem = fileSystemAdapter;
			this.Convention = convention;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveMapping(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Mapping");
			this.VersionControl.DeleteMapping(branch);

			this.TextOutput.WriteVerbose("Deleting Solution");
			this.FileSystem.DeleteDirectory(this.Convention.GetLocalPath(branch));

			this.TextOutput.WriteVerbose("Removing Application");
			this.AdeNet.RemoveApplication(branch);
		}
		#endregion
	}
}