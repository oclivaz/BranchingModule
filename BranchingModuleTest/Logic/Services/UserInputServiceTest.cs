using System;
using BranchingModule.Logic;
using BranchingModuleTest.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BranchingModuleTest.Logic.Services
{
	[TestClass]
	public class UserInputServiceTest : BranchingModuleTestBase
	{
		#region Constants
		private const string SOME_TEXT = "Some text";
		#endregion

		#region Properties
		private IUserInputService UserInputService { get; set; }
		#endregion

		#region Initialize and Cleanup
		[TestInitialize]
		public void InitializeTest()
		{
			this.UserInputService = new UserInputService();
		}
		#endregion

		#region Tests
		[TestMethod]
		public void TestRequestConfirmation_one_Provider_confirmation()
		{
			// Arrange
			IUserInputProvider inputProvider = Substitute.For<IUserInputProvider>();
			inputProvider.RequestConfirmation(SOME_TEXT).Returns(true);
			this.UserInputService.SetProvider(inputProvider);

			// Act
			bool bConfirmed = this.UserInputService.RequestConfirmation(SOME_TEXT);

			// Assert
			Assert.IsTrue(bConfirmed);
		}

		[TestMethod]
		public void TestRequestConfirmation_one_Provider_no_confirmation()
		{
			// Arrange
			IUserInputProvider inputProvider = Substitute.For<IUserInputProvider>();
			inputProvider.RequestConfirmation(SOME_TEXT).Returns(false);
			this.UserInputService.SetProvider(inputProvider);

			// Act
			bool bConfirmed = this.UserInputService.RequestConfirmation(SOME_TEXT);

			// Assert
			Assert.IsFalse(bConfirmed);
		}

		[TestMethod]
		public void TestRequestConfirmation_two_Providers()
		{
			// Arrange
			IUserInputProvider inputProvider1 = Substitute.For<IUserInputProvider>();
			IUserInputProvider inputProvider2 = Substitute.For<IUserInputProvider>();
			this.UserInputService.SetProvider(inputProvider1);
			this.UserInputService.SetProvider(inputProvider2);

			// Act
			this.UserInputService.RequestConfirmation(SOME_TEXT);

			// Assert
			inputProvider1.DidNotReceive().RequestConfirmation(Arg.Any<string>());
			inputProvider2.Received().RequestConfirmation(SOME_TEXT);
		}

		[TestMethod]
		public void TestRequestConfirmation_no_Provider()
		{
			// Act
			ExceptionAssert.Throws<InvalidOperationException>(() => this.UserInputService.RequestConfirmation(SOME_TEXT), "No UserInputProvider has been set");
		}
		#endregion
	}
}