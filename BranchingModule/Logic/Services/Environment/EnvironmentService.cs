using System;

namespace BranchingModule.Logic
{
	internal class EnvironmentService : IEnvironmentService
	{
		#region Properties
		public IFileExecutionService FileExecution { get; set; }
		public IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public EnvironmentService(IFileExecutionService fileExecutionService, IConvention convention)
		{
			if(fileExecutionService == null) throw new ArgumentNullException("fileExecutionService");
			if(convention == null) throw new ArgumentNullException("convention");

			this.FileExecution = fileExecutionService;
			this.Convention = convention;
		}
		#endregion

		#region Publics
		public void OpenSolution(BranchInfo branch)
		{
			this.FileExecution.StartProcess(Executables.EXPLORER, this.Convention.GetSolutionFile(branch));
		}

		public void OpenWeb(BranchInfo branch)
		{
			this.FileExecution.StartProcess(Executables.INTERNET_EXPLORER, string.Format("http://localhost/{0}", this.Convention.GetApplicationName(branch)));
		}
		#endregion
	}
}