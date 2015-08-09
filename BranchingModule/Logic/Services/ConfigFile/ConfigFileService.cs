using System;
using System.Text;

namespace BranchingModule.Logic
{
	internal class ConfigFileService : IConfigFileService
	{
		#region Properties
		private IConvention Convention { get; set; }
		private ISettings Settings { get; set; }
		private IFileWriter FileWriter { get; set; }
		#endregion

		#region Constructors
		public ConfigFileService(IConvention convention, ISettings settings, IFileWriter fileWriter)
		{
			if(convention == null) throw new ArgumentNullException("convention");

			this.Convention = convention;
			this.Settings = settings;
			this.FileWriter = fileWriter;
		}
		#endregion

		#region Publics
		public void CreateIndivConfig(BranchInfo branch)
		{
			ITeamProjectSettings teamProjectSettings = this.Settings.GetTeamProjectSettings(branch.TeamProject);

			string strContent = String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
    <appSettings>
        <add key=""ConnectionString"" value=""server=localhost;Database={0};User ID=sa;Pwd=password-123"" />
        <add key=""SMTPServer"" value=""193.135.175.18"" /> 
        <add key=""AppTitle"" value=""{1} Release {2}"" />
        <add key=""Company"" value=""M-S¦Pension"" />
        <add key=""EnableUserThemes"" value=""true"" />
        <add key=""SessionKeepAlive"" value=""true"" />
    </appSettings>", teamProjectSettings.LocalDB, branch.TeamProject, branch.Name);

			string strIndivConfig = String.Format(@"{0}\Web\Indiv\indiv.config", this.Convention.GetLocalPath(branch));
			this.FileWriter.Write(strIndivConfig, strContent, Encoding.UTF8);
		}
		#endregion
	}
}