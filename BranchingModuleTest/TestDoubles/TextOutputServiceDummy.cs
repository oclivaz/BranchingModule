using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	class TextOutputServiceDummy : ITextOutputService
	{
		public void RegisterListener(ITextOutputListener listener)
		{
		}

		public void WriteVerbose(string strText)
		{
		}
	}
}
