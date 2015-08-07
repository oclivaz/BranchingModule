using BranchingModuleTest.TestDoubles;
using Ninject.Activation;

namespace BranchingModule.Logic
{
	class TestSettingsFactory : Provider<ISettings>
	{
		protected override ISettings CreateInstance(IContext context)
		{
			return new SettingsDummy();
		}
	}
}
