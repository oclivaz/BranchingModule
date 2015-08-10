using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.DependencyInjection
{
	[TestClass]
	public class DependencyInjectionTest
	{
		[TestMethod]
		public void TestCreate_AddMappingController()
		{
			// Arrange
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();

			// Act
			factory.Get<AddMappingController>();
		}
	}
}
