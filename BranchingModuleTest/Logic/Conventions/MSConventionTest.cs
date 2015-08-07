using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Logic.Conventions
{
	[TestClass]
	public class MSConventionTest
	{
		#region Fields
		private MSConvention MSConvention;
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.MSConvention = new MSConvention();
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSConvention.GetLocalPath(new BranchInfo("AkisBV", "2.5.3"));

			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_2_5_3", strLocalPath);
		}

		[TestMethod]
		public void TestGetSeverPath()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath(new BranchInfo("AkisBV", "2.5.3"));

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/2.5.3/Source", strServerPath);
		}
		#endregion
	}
}