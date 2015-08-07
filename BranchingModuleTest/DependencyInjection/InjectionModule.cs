using BranchingModule.Logic;
using Ninject.Modules;

namespace BranchingModuleTest.DependencyInjection
{
	internal class InjectionModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
	}
}
