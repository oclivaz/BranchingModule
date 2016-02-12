using BranchingModule.Logic;
using BranchingModuleTest.Base;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class NewDatabaseControllerTest : BranchingModuleTestBase
	{
		#region Properties
		private NewDatabaseController NewDatabaseController { get; set; }
		private IDatabaseService Database { get; set; }
		private IAblageService Ablage { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.Database = Substitute.For<IDatabaseService>();
			this.Ablage = Substitute.For<IAblageService>();

			this.NewDatabaseController = new NewDatabaseController(this.Database, this.Ablage, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCreateDatabase()
		{
			// Act
			this.NewDatabaseController.CreateDatabase(AKISBV_5_0_35);

			// Assert
			this.Database.Received().Create(AKISBV_5_0_35);
			this.Ablage.Received().Reset(AKISBV_5_0_35);
		}
		#endregion
	}
}