using System;

namespace BranchingModule.Logic.Adapters
{
	internal class SourceControlAdapter : ISourceControlAdapter
	{
		#region Properties
		private ISettings Settings { get; set; }

		private IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public SourceControlAdapter(IConvention convention, ISettings settings)
		{
			if(settings == null) throw new ArgumentNullException("settings");
			if(convention == null) throw new ArgumentNullException("convention");

			this.Convention = convention;
			this.Settings = settings;
		}
		#endregion

		#region Publics
		public void CreateMapping(string localPath, string serverPath)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}