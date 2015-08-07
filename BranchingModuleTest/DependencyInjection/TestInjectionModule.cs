using System;
using BranchingModule.Logic;
using Ninject.Modules;

namespace BranchingModuleTest.Framework.Modules
{
	internal class TestInjectionModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISettings>().ToProvider(new TestSettingsFactory()).InSingletonScope();
		}
	}
}
