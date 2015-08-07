using System.IO;
using Newtonsoft.Json;
using Ninject.Activation;

namespace BranchingModule.Logic
{
	class SettingsFactory : Provider<ISettings>
	{
		protected override ISettings CreateInstance(IContext context)
		{
			SettingsDTO settingsDTO = JsonConvert.DeserializeObject<SettingsDTO>(File.OpenText(Settings.DEFAULT_SETTINGS_FILE).ReadToEnd());
			return new Settings(settingsDTO);
		}
	}
}
