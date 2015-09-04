using System;

namespace BranchingModule.Logic
{
	internal class UserInputService : IUserInputService
	{
		#region Properties
		private IUserInputProvider UserInputProvider { get; set; }
		#endregion

		#region Publics
		public void SetProvider(IUserInputProvider inputProvider)
		{
			if(inputProvider == null) throw new ArgumentNullException("inputProvider");

			this.UserInputProvider = inputProvider;
		}

		public bool RequestConfirmation(string strMessageToConfirm)
		{
			if(strMessageToConfirm == null) throw new ArgumentNullException("strMessageToConfirm");
			if(this.UserInputProvider == null) throw new InvalidOperationException("No UserInputProvider has been set");

			return this.UserInputProvider.RequestConfirmation(strMessageToConfirm);
		}
		#endregion
	}
}