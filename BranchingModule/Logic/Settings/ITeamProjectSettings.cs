﻿namespace BranchingModule.Logic
{
	public interface ITeamProjectSettings
	{
		string LocalDB { get; }
		string RefDB { get; }
		string[] AditionalReferences { get; }
		string AppConfigPath { get; }
	}
}
