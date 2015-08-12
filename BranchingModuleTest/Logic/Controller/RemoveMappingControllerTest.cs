﻿using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveMappingControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private RemoveMappingController RemoveMappingController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		private IFileSystemAdapter FileSystem { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.AdeNet = Substitute.For<IAdeNetService>();
			this.FileSystem = Substitute.For<IFileSystemAdapter>();

			this.RemoveMappingController = new RemoveMappingController(this.VersionControl, this.AdeNet, this.FileSystem, new ConventionDummy(), new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRemoveMapping()
		{
			// Act
			this.RemoveMappingController.RemoveMapping(AKISBV_5_0_35);

			// Assert
			this.VersionControl.Received().DeleteMapping(AKISBV_5_0_35);
			this.FileSystem.Received().DeleteDirectory(LOCAL_PATH_AKISBV_5_0_35);
			this.AdeNet.Received().RemoveApplication(AKISBV_5_0_35);
		}
		#endregion
	}
}