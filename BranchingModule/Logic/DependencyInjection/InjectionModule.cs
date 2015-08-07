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
			Bind<IAdeNetAdapter>().To<AdeNetAdapter>();
			Bind<IBuildEngineAdapter>().To<MSBuildAdapter>();
			Bind<IConfigFileService>().To<ConfigFileService>();
			Bind<IFileWriter>().To<FileWriter>();
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
		#endregion
	}
}