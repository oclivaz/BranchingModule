using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Ninject.Activation;

namespace BranchingModuleTest.DependencyInjection
{
	class SettingsFactory : Provider<ISettings>
	{
		protected override ISettings CreateInstance(IContext context)
		{
			return new SettingsDummy();
		}
	}
}
