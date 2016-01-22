namespace BranchingModule.Logic
{
	public interface ITeamProjectSettings
	{
		string LocalDB { get; }
		string RefDB { get; }
		string[] AditionalPackages { get; }
	}
}
