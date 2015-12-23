using System;

namespace BranchingModule.Logic
{
	internal class OpenSolutionController
	{
		#region Properties
		private IEnvironmentService Environment { get; set; }
		private IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public OpenSolutionController(IEnvironmentService environmentService, IConvention convention)
		{
			if(environmentService == null) throw new ArgumentNullException("environmentService");
			if(convention == null) throw new ArgumentNullException("convention");

			this.Environment = environmentService;
			this.Convention = convention;
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