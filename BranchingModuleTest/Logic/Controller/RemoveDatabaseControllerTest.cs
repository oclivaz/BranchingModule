using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class RemoveDatabaseControllerTest : BranchingModuleTestBase
	{
		#region Constants
		private const string SOME_FILE = @"c:\temp\some_file";
		#endregion

		#region Properties
		private RemoveDatabaseController RemoveDatabaseController { get; set; }
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();

			this.RemoveDatabaseController = new RemoveDatabaseController(this.Database, this.Ablage, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRestore_with_no_Ablage_yet()
		{
			// Act
			this.RemoveDatabaseController.RemoveDatabase(AKISBV_5_0_35);

			// Assert
			this.Database.Received().Drop(AKISBV_5_0_35);
			this.Ablage.Received().Remove(AKISBV_5_0_35);
		}
		#endregion
	}
}