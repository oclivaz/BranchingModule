namespace BranchingModule.Logic
{
	internal class OpenWebController
	{
		#region Properties
		private IEnvironmentService Environment { get; set; }
		#endregion

		#region Constructors
		public OpenWebController(IEnvironmentService environmentService)
		{
			this.Environment = environmentService;
		}
		#endregion

		#region Publics
		public void OpenWeb(BranchInfo branch)
		{
			this.Environment.OpenWeb(branch);
		}
		#endregion
	}
}