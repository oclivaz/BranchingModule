using BranchingModule.Logic;

namespace BranchingModuleTest.TestDoubles
{
	internal class UserInputServiceDummy : IUserInputService
	{
		public void SetProvider(IUserInputProvider inputProvider)
		{
			// ...meh...
		}

		public bool RequestConfirmation(string strMessageToConfirm)
		{
			return true;
		}
	}
}
