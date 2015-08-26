using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Base
{
	public static class ExceptionAssert
	{
		public static void Throws<T>(Action task, string expectedContent) where T : Exception
		{
			try
			{
				task();
			}
			catch(T ex)
			{
				StringAssert.Contains(ex.Message, expectedContent);
			}
			catch(Exception ex)
			{
				Assert.Fail("Expected Exception of type {0}, but got {1}", typeof(T).Name, ex.GetType().Name);
			}
		}
	}
}
