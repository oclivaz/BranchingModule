using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.DependencyInjection
{
	[TestClass]
	public class ControllerFactoryTest
	{
		[TestMethod]
		public void TestCreate_AddMappingController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<AddMappingController>();
		}

		[TestMethod]
		public void TestCreate_AddReleasebranchController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<AddReleasebranchController>();
		}

		[TestMethod]
		public void TestCreate_MergeBugfixController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<MergeBugfixController>();
		}

		[TestMethod]
		public void TestCreate_OpenSolutionController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<OpenSolutionController>();
		}

		[TestMethod]
		public void TestCreate_RemoveMappingController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<RemoveMappingController>();
		}

		[TestMethod]
		public void TestCreate_RemoveReleasebranchController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<RemoveReleasebranchController>();
		}

		[TestMethod]
		public void TestCreate_GetLatestController()
		{
			// Arrange
			IControllerFactory factory = new ControllerFactory();

			// Act
			factory.Get<GetLatestController>();
		}
	}
}
