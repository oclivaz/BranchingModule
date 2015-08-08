using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace BranchingModuleTest.DependencyInjection
{
	[TestClass]
	public class InjectionModuleIntegrationTest
	{
		[TestMethod]
		public void TestCreate_AddMappingController()
		{
			// Arrange
			IKernel kernel = new StandardKernel(new BranchingModule.Logic.InjectionModule());

			// Act
			kernel.Get<AddMappingController>();

		}
	}
}
