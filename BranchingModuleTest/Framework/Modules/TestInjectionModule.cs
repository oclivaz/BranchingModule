using System;
using Ninject.Modules;

namespace BranchingModuleTest.Framework.Modules
{
	internal class TestInjectionModule : NinjectModule
	{
		public override void Load()
		{
			throw new NotImplementedException("Dependencies should be delivered in the Unit Test");
		}
	}
}
