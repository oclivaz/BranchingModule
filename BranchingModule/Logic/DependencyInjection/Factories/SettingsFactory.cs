using System.IO;
using Newtonsoft.Json;
using Ninject.Activation;

namespace BranchingModule.Logic
{
	class SettingsFactory : Provider<ISettings>
	{
		protected override ISettings CreateInstance(IContext context)
		{
			IFileSystemService fileSystem = new FileSystemService();
			SettingsDTO settingsDTO = JsonConvert.DeserializeObject<SettingsDTO>(fileSystem.ReadAllText(Settings.DEFAULT_SETTINGS_FILE));
			return new Settings(settingsDTO);
		}
	}
}
