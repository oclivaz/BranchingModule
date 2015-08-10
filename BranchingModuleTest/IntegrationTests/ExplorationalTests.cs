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
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();
			IDumpService service = factory.Get<DumpService>();

			service.RestoreDump(new BranchInfo("AkisBV", "5.0.35"));
		}
		#endregion
	}
}