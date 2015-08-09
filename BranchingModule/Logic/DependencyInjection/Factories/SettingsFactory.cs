using Newtonsoft.Json;
using Ninject.Activation;

namespace BranchingModule.Logic
{
	internal class SettingsFactory : Provider<ISettings>
	{
		#region Protecteds
		protected override ISettings CreateInstance(IContext context)
		{
			IFileSystemService fileSystem = new FileSystemService();
			SettingsDTO settingsDTO = JsonConvert.DeserializeObject<SettingsDTO>(fileSystem.ReadAllText(Settings.DEFAULT_SETTINGS_FILE));
			return new Settings(settingsDTO);
		}
		#endregion
	}
}