using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class AblageServiceTest : BranchingModuleTestBase
	{
		#region Properties
		private IAblageService Ablage { get; set; }

		private IFileSystemAdapter FileSystemAdapter { get; set; }

		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.FileSystemAdapter = Substitute.For<IFileSystemAdapter>();

			this.Ablage = new AblageService(this.FileSystemAdapter, new ConventionDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestReset_no_Ablage_yet()
		{
			// Arrange
			this.FileSystemAdapter.Exists(ABLAGE_PATH_AKISBV_5_0_35).Returns(false);

			// Act
			this.Ablage.Reset(AKISBV_5_0_35);

			// Assert
			this.FileSystemAdapter.Received().CreateDirectory(ABLAGE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestReset_Ablage_already_present()
		{
			// Arrange
			this.FileSystemAdapter.Exists(ABLAGE_PATH_AKISBV_5_0_35).Returns(true);

			// Act
			this.Ablage.Reset(AKISBV_5_0_35);

			// Assert
			this.FileSystemAdapter.Received().EmptyDirectory(ABLAGE_PATH_AKISBV_5_0_35);
			this.FileSystemAdapter.DidNotReceive().CreateDirectory(ABLAGE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestRemove_with_ablage_present()
		{
			// Arrange
			this.FileSystemAdapter.Exists(ABLAGE_PATH_AKISBV_5_0_35).Returns(true);

			// Act
			this.Ablage.Remove(AKISBV_5_0_35);

			// Assert
			this.FileSystemAdapter.Received().DeleteDirectory(ABLAGE_PATH_AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestRemove_with_no_ablage_present()
		{
			// Arrange
			this.FileSystemAdapter.Exists(ABLAGE_PATH_AKISBV_5_0_35).Returns(false);

			// Act
			this.Ablage.Remove(AKISBV_5_0_35);

			// Assert
			this.FileSystemAdapter.DidNotReceive().DeleteDirectory(ABLAGE_PATH_AKISBV_5_0_35);
		}
		#endregion
	}
}