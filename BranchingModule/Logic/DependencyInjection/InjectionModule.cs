using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class InjectionModule : NinjectModule
	{
		#region Publics
		public override void Load()
		{
			Bind<IConvention>().To<MSConvention>().InSingletonScope();
			Bind<ISourceControlService>().To<TeamFoundationService>().InSingletonScope();
			Bind<IAdeNetService>().To<AdeNetService>();
			Bind<IBuildEngineService>().To<MsBuildService>();
			Bind<IConfigFileService>().To<ConfigFileService>();
			Bind<IFileWriter>().To<FileWriter>();
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
		#endregion
	}
}