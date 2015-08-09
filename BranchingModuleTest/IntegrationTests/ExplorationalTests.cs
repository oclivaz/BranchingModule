using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

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
			IKernel kernel = new StandardKernel(new InjectionModule());
			IDumpService service = kernel.Get<DumpService>();

			service.RestoreDump(new BranchInfo("AkisBV", "5.0.35"));
		}
		#endregion
	}
}