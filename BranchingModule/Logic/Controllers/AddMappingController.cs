using System;

namespace BranchingModule.Logic
{
	internal class AddMappingController
	{
		#region Properties
		private ISourceControlAdapter SourceControl { get; set; }
		private IAdeNetAdapter AdeNet { get; set; }
		#endregion

		#region Constructors
		public AddMappingController(ISourceControlAdapter sourceControlAdapter, IAdeNetAdapter adeNetAdapter)
		{
			if(sourceControlAdapter == null) throw new ArgumentNullException("sourceControlAdapter");

			this.SourceControl = sourceControlAdapter;
			this.AdeNet = adeNetAdapter;
		}
		#endregion

		#region Publics
		public void Process(string strTeamProject, string strBranch)
		{
			if(strTeamProject == null) throw new ArgumentNullException("strTeamProject");
			if(strBranch == null) throw new ArgumentNullException("strBranch");

			this.SourceControl.CreateMapping(strTeamProject, strBranch);

			this.AdeNet.InstallPackages(strTeamProject, strBranch);
		}
		#endregion
	}
}