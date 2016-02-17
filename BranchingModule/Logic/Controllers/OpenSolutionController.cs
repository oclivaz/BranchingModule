using System;

namespace BranchingModule.Logic
{
	internal class OpenSolutionController
	{
		#region Properties
		private IEnvironmentService Environment { get; set; }
		#endregion

		#region Constructors
		public OpenSolutionController(IEnvironmentService environmentService)
		{
			if(environmentService == null) throw new ArgumentNullException("environmentService");

			this.Environment = environmentService;
		}
		#endregion

		#region Publics
		public void OpenSolution(BranchInfo branch)
		{
			this.Environment.OpenSolution(branch);
		}
		#endregion
	}
}