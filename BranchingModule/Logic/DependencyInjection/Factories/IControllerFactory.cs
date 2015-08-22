namespace BranchingModule.Logic
{
	public interface IControllerFactory
	{
		T Get<T>();
	}
}
