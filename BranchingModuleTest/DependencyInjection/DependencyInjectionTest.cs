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
			IObjectFactory factory = new ObjectFactory();

			// Act
			factory.Get<AddMappingController>();
		}
	}
}
