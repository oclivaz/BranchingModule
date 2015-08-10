using System;

namespace BranchingModule.Logic
{
	internal class RemoveReleasebranchController
	{
		#region Properties
		private ISourceControlService SourceControl { get; set; }
		private ITextOutputService TextOutput { get; set; }
		#endregion

		#region Constructors
		public RemoveReleasebranchController(ISourceControlService sourceControlService, ITextOutputService textOutputService)
		{
			if(sourceControlService == null) throw new ArgumentNullException("sourceControlService");
			if(textOutputService == null) throw new ArgumentNullException("textOutputService");

			this.SourceControl = sourceControlService;
			this.TextOutput = textOutputService;
		}
		#endregion

		#region Publics
		public void RemoveReleasebranch(BranchInfo branch)
		{
			this.TextOutput.WriteVerbose("Deleting Branch");
			this.SourceControl.DeleteBranch(branch);
		}
		#endregion
	}
}