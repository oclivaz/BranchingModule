namespace BranchingModule.Logic
{
	public interface IUserInputProvider
	{
		bool RequestConfirmation(string strMessageToConfirm);
	}
}
