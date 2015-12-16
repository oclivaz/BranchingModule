using Ninject;
using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class ControllerFactory
	{
		#region Fields
		private static IKernel _kernel;
		#endregion

		#region Properties
		private static IKernel NinjectKernel
		{
			get { return _kernel ?? (_kernel = new StandardKernel(new InjectionModule())); }
		}
		#endregion

		#region Publics
		public static T Get<T>()
		{
			return NinjectKernel.Get<T>();
		}
		#endregion

		#region Class InjectionModule
		private class InjectionModule : NinjectModule
		{
			#region Publics
			public override void Load()
			{
				// Adapters
				Bind<ITeamFoundationVersionControlAdapter>().To<TeamFoundationVersionControlAdapter>();

				// Services
				Bind<IVersionControlService>().To<TeamFoundationService>().InSingletonScope();
				Bind<IAdeNetService>().To<AdeNetService>().InSingletonScope();
				Bind<IBuildEngineService>().To<MsBuildService>().InSingletonScope();
				Bind<IConfigFileService>().To<ConfigFileService>().InSingletonScope();
				Bind<IDatabaseService>().To<DatabaseService>().InSingletonScope();
				Bind<IDumpRepositoryService>().To<DumpRepositoryService>().InSingletonScope();
				Bind<IFileSystemAdapter>().To<FileSystemAdapter>().InSingletonScope();
				Bind<ISQLServerAdapter>().To<MssqlServerAdapter>();
				Bind<ITextOutputService>().To<TextOutputService>().InSingletonScope();
				Bind<IUserInputService>().To<UserInputService>().InSingletonScope();
				Bind<IFileExecutionService>().To<FileExecutionService>().InSingletonScope();

				// Other
				Bind<IConvention>().To<MSConvention>().InSingletonScope();
				Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
			}
			#endregion
		}
		#endregion
	}
}