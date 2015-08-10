using System;

namespace BranchingModule.Logic
{
	internal class MsBuildService : IBuildEngineService
	{
		#region Properties
		public IFileExecutionService FileExecution { get; set; }
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		#endregion

		#region Constructors
		public MsBuildService(IFileExecutionService fileExecutionService, IConvention convention, ISettings settings)
		{
			if(convention == null) throw new ArgumentNullException("convention");
			if(settings == null) throw new ArgumentNullException("settings");

			this.FileExecution = fileExecutionService;
			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void Build(BranchInfo branch)
		{
			string strArguments = string.Format(@"{0}\{1}.sln /t:Build", this.Convention.GetLocalPath(branch), branch.TeamProject);
			this.FileExecution.Execute(this.Settings.MSBuildExePath, strArguments);
		}
		#endregion
	}
}