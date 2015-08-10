using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Logic
{
	[TestClass]
	public class BranchInfoTest
	{
		[TestMethod]
		public void TestToString()
		{
			// Arrange
			BranchInfo branchInfo = new BranchInfo("AkisBV", "1.2.3");

			// Act
			string strBranchinfog = branchInfo.ToString();

			// Assert
			Assert.AreEqual("AkisBV 1.2.3", strBranchinfog);
		}
	}
}
