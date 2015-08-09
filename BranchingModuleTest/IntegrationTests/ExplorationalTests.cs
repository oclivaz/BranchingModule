using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.IntegrationTests
{
	[Ignore]
	[TestClass]
	public class ExplorationalTests
	{
		#region Tests
		[TestMethod]
		public void Test()
		{
			IObjectFactory factory = new ObjectFactory();
			IDumpService service = factory.Get<DumpService>();

			service.RestoreDump(new BranchInfo("AkisBV", "5.0.35"));
		}
		#endregion
	}
}