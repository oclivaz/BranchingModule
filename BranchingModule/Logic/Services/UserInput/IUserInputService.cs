namespace BranchingModule.Logic
{
	public interface IUserInputService
	{
		void SetProvider(IUserInputProvider inputProvider);
		bool RequestConfirmation(string strMessageToConfirm);
	}
}