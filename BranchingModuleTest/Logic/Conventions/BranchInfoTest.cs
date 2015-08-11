using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Logic
{
	[TestClass]
	public class BranchInfoTest
	{
		#region Constants
		private static readonly BranchInfo AKISBV_1_2_3 = new BranchInfo("AkisBV", "1.2.3");
		#endregion

		#region Tests
		[TestMethod]
		public void TestToString()
		{
			// Arrange
			BranchInfo branchInfo = AKISBV_1_2_3;

			// Act
			string strBranchinfog = branchInfo.ToString();

			// Assert
			Assert.AreEqual("AkisBV 1.2.3", strBranchinfog);
		}
		#endregion
	}
}