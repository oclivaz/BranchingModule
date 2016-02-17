﻿using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class TextOutputServiceDummy : ITextOutputService
	{
		public void RegisterListener(ITextOutputListener listener)
		{
			// TextOutputServiceDummpy don't care
		}

		public void WriteVerbose(string strText)
		{
			// TextOutputServiceDummpy don't care
		}

		public void Write(string strText)
		{
			// TextOutputServiceDummpy don't care
		}

		public void UnregisterListener(ITextOutputListener listener)
		{
			// TextOutputServiceDummpy don't care
		}
	}
}
