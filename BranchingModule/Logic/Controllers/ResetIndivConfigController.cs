using System;

namespace BranchingModule.Logic
{
	internal class ResetIndivConfigController
	{
		#region Properties
		public IConfigFileService ConfigFile { get; set; }
		#endregion

		#region Constructors
		public ResetIndivConfigController(IConfigFileService configFile)
		{
			if(configFile == null) throw new ArgumentNullException("configFile");

			this.ConfigFile = configFile;
		}
		#endregion

		#region Publics
		public void ResetIndivConfig(BranchInfo branch)
		{
			this.ConfigFile.CreateIndivConfig(branch);
		}
		#endregion
	}
}