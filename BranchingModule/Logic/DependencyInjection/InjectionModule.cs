using Ninject.Modules;

namespace BranchingModule.Logic
{
	internal class InjectionModule : NinjectModule
	{
		#region Publics
		public override void Load()
		{
			Bind<ISettings>().ToProvider(new SettingsFactory()).InSingletonScope();
		}
		#endregion
	}
}