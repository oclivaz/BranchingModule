using BranchingModule.Logic;
using BranchingModuleTest.TestDoubles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Controller
{
	[TestClass]
	public class AddReleasebranchControllerTest
	{
		#region Constants
		private static readonly BranchInfo AKISBV_5_0_35 = new BranchInfo("AkisBVBL", "1.2.3");
		#endregion

		#region Properties
		public IDatabaseService Database { get; set; }
		private AddReleasebranchController AddReleasebranchController { get; set; }
		private IVersionControlService VersionControl { get; set; }
		private IAdeNetService AdeNet { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.VersionControl = Substitute.For<IVersionControlService>();
			this.Database = Substitute.For<IDatabaseService>();
			this.AdeNet = Substitute.For<IAdeNetService>();

			this.AddReleasebranchController = new AddReleasebranchController(this.VersionControl, this.Database, this.AdeNet, new TextOutputServiceDummy());
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestCreateBranch()
		{
			// Act
			this.AddReleasebranchController.AddReleasebranch(AKISBV_5_0_35);

			// Assert
			this.VersionControl.Received().CreateBranch(AKISBV_5_0_35);
			this.Database.Received().InstallBuildserverDump(AKISBV_5_0_35);
			this.AdeNet.Received().CreateBuildDefinition(AKISBV_5_0_35);
		}
		#endregion
	}
}