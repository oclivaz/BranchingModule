using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class InjectionModule : NinjectModule
	{
		#region Publics
		public override void Load()
		{
			// Services
			Bind<ISourceControlService>().To<TeamFoundationService>().InSingletonScope();
			Bind<IAdeNetService>().To<AdeNetService>().InSingletonScope();
			Bind<IBuildEngineService>().To<MsBuildService>().InSingletonScope();
			Bind<IConfigFileService>().To<ConfigFileService>().InSingletonScope();

			// Other
			Bind<IConvention>().To<MSConvention>().InSingletonScope();
			Bind<IFileWriter>().To<FileWriter>().InSingletonScope();
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
		#endregion
	}
}