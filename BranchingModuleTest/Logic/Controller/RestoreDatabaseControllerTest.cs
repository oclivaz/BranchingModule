using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RestoreDatabaseControllerTest : BranchingModuleTestBase
	{
		#region Constants
		private const string SOME_FILE = @"c:\temp\some_file";
		#endregion

		#region Properties
		private RestoreDatabaseController RestoreDatabaseController { get; set; }
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();

			this.RestoreDatabaseController = new RestoreDatabaseController(this.Database, this.Ablage, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRestore_with_no_Ablage_yet()
		{
			// Act
			this.RestoreDatabaseController.RestoreDatabase(AKISBV_5_0_35);

			// Assert
			this.Database.Received().Restore(AKISBV_5_0_35);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
		}

		[TestMethod]
		public void TestRestore_with_file()
		{
			// Act
			this.RestoreDatabaseController.RestoreDatabase(AKISBV_5_0_35, SOME_FILE);

			// Assert
			this.Database.Received().Restore(AKISBV_5_0_35, SOME_FILE);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
		}
		#endregion
	}
}