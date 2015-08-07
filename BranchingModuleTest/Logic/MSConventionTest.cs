using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Logic
{
	[TestClass]
	public class MSConventionTest
	{
		private MSConvention MSConvention;

		[TestInitialize]
		public void InitializeTest()
		{
			this.MSConvention = new MSConvention();
		}

		[TestMethod]
		public void TestGetLocalPath()
		{
			// Act
			string strLocalPath = this.MSConvention.GetLocalPath("AkisBV", "2.5.3");
			
			// Assert
			Assert.AreEqual(@"C:\inetpub\wwwroot\AkisBV_2_5_3", strLocalPath);
		}

		[TestMethod]
		public void TestGetSeverPath()
		{
			// Act
			string strServerPath = this.MSConvention.GetServerPath("AkisBV", "2.5.3");

			// Assert
			Assert.AreEqual(@"$/AkisBV/Release/2.5.3/Source", strServerPath);
		}
	}
}
