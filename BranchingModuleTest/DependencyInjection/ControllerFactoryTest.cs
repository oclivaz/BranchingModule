﻿using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.DependencyInjection
{
	[TestClass]
	public class ControllerFactoryTest
	{
		#region Tests
		[TestMethod]
		public void TestCreate_AddMappingController()
		{
			// Act
			ControllerFactory.Get<AddMappingController>();
		}

		[TestMethod]
		public void TestCreate_AddReleasebranchController()
		{
			// Act
			ControllerFactory.Get<AddReleasebranchController>();
		}

		[TestMethod]
		public void TestCreate_MergeBugfixController()
		{
			// Act
			ControllerFactory.Get<MergeBugfixController>();
		}

		[TestMethod]
		public void TestCreate_OpenSolutionController()
		{
			// Act
			ControllerFactory.Get<OpenSolutionController>();
		}

		[TestMethod]
		public void TestCreate_OpenWebController()
		{
			// Act
			ControllerFactory.Get<OpenWebController>();
		}

		[TestMethod]
		public void TestCreate_RemoveMappingController()
		{
			// Act
			ControllerFactory.Get<RemoveMappingController>();
		}

		[TestMethod]
		public void TestCreate_RemoveReleasebranchController()
		{
			// Act
			ControllerFactory.Get<RemoveReleasebranchController>();
		}

		[TestMethod]
		public void TestCreate_GetLatestController()
		{
			// Act
			ControllerFactory.Get<GetLatestController>();
		}

		[TestMethod]
		public void TestCreate_NewDatabaseController()
		{
			// Act
			ControllerFactory.Get<NewDatabaseController>();
		}

		[TestMethod]
		public void TestCreate_BackupDatabaseController()
		{
			// Act
			ControllerFactory.Get<BackupDatabaseController>();
		}

		[TestMethod]
		public void TestCreate_RestoreDatabaseController()
		{
			// Act
			ControllerFactory.Get<RestoreDatabaseController>();
		}

		[TestMethod]
		public void TestCreate_ShowReleasebranchesController()
		{
			// Act
			ControllerFactory.Get<ShowReleasebranchesController>();
		}

		[TestMethod]
		public void TestCreate_ResetIndivConfigController()
		{
			// Act
			ControllerFactory.Get<ResetIndivConfigController>();
		}

		[TestMethod]
		public void TestCreate_RemoveDatabaseController()
		{
			// Act
			ControllerFactory.Get<RemoveDatabaseController>();
		}
		#endregion
	}
}