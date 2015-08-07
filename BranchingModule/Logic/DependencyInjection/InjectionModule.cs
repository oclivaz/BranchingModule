using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class InjectionModule : NinjectModule
	{
		#region Publics
		public override void Load()
		{
			Bind<IConvention>().To<MSConvention>().InSingletonScope();
			Bind<ISourceControlAdapter>().To<TeamFoundationAdapter>().InSingletonScope();
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
		#endregion
	}
}