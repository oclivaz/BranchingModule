﻿using Ninject;
using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class ObjectFactory : IObjectFactory
	{
		#region Fields
		private IKernel _kernel;
		#endregion

		#region Properties
		private IKernel NinjectKernel
		{
			get { return _kernel ?? (_kernel = new StandardKernel(new InjectionModule())); }
		}
		#endregion

		#region Publics
		public T Get<T>()
		{
			return this.NinjectKernel.Get<T>();
		}
		#endregion

		#region Class InjectionModule
		private class InjectionModule : NinjectModule
		{
			#region Publics
			public override void Load()
			{
				// Services
				Bind<ISourceControlService>().To<TeamFoundationService>().InSingletonScope();
				Bind<IAdeNetService>().To<AdeNetService>().InSingletonScope();
				Bind<IBuildEngineService>().To<MsBuildService>().InSingletonScope();
				Bind<IConfigFileService>().To<ConfigFileService>().InSingletonScope();
				Bind<IDumpService>().To<DumpService>().InSingletonScope();
				Bind<IDumpRepositoryService>().To<DumpRepositoryService>().InSingletonScope();

				// Other
				Bind<IConvention>().To<MSConvention>().InSingletonScope();
				Bind<IFileWriter>().To<FileWriter>().InSingletonScope();
				Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
			}
			#endregion
		}
		#endregion
	}
}