using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Logic
{
	[TestClass]
	public class BranchInfoTest : BranchingModuleTestBase
	{
		#region Tests
		[TestMethod]
		public void TestToString()
		{
			// Act
			string strBranchinfog = AKISBV_5_0_35.ToString();

			// Assert
			Assert.AreEqual("AkisBV 5.0.35", strBranchinfog);
		}

		[TestMethod]
		public void TestCreate()
		{
			// Act
			BranchInfo branch = BranchInfo.Create("AkisBV", "5.0.35");

			// Assert
			Assert.AreEqual(AKISBV_5_0_35, branch);
		}

		[TestMethod]
		public void TestCreate_no_Branchname_provided()
		{
			// Act
			BranchInfo branch = BranchInfo.Create("AkisBV", null);

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		public void TestCreate_empty_Branchname_provided()
		{
			// Act
			BranchInfo branch = BranchInfo.Create("AkisBV", "");

			// Assert
			Assert.AreEqual(AKISBV_MAIN, branch);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCreate_no_Teamproject_provided()
		{
			// Act
			BranchInfo.Create(null, "5.0.35");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TestCreate_empty_Teamproject_provided()
		{
			// Act
			BranchInfo.Create("", "5.0.35");
		}
		#endregion
	}
}