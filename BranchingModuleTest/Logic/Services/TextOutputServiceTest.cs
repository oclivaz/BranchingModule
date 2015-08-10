using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class TextOutputServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private const string SOME_TEXT = "Some text";
		#endregion

		#region Properties
		private ITextOutputService TextOutputService { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.TextOutputService = new TextOutputService();
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestWriteVerbose_one_Listener()
		{
			// Arrange
			ITextOutputListener listener = Substitute.For<ITextOutputListener>();
			this.TextOutputService.RegisterListener(listener);

			// Act
			this.TextOutputService.WriteVerbose(SOME_TEXT);

			// Assert
			listener.Received().WriteVerbose(SOME_TEXT);
		}

		[TestMethod]
		public void TestWriteVerbose_two_Listeners()
		{
			// Arrange
			ITextOutputListener listener1 = Substitute.For<ITextOutputListener>();
			ITextOutputListener listener2 = Substitute.For<ITextOutputListener>();
			this.TextOutputService.RegisterListener(listener1);
			this.TextOutputService.RegisterListener(listener2);

			// Act
			this.TextOutputService.WriteVerbose(SOME_TEXT);

			// Assert
			listener1.Received().WriteVerbose(SOME_TEXT);
			listener2.Received().WriteVerbose(SOME_TEXT);
		}

		[TestMethod]
		public void TestWriteVerbose_no_Listener()
		{
			// Act
			this.TextOutputService.WriteVerbose(SOME_TEXT);
		}
		#endregion
	}
}