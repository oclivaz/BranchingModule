using BranchingModule.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BranchingModuleTest.Base
{
	[TestClass]
	public class BranchingModuleTestBase
	{
		#region Protecteds
		protected ITeamProjectSettings TeamProjectSettings(string strLocalDB, string strRefDB)
		{
			TeamProjectSettingsDTO dto = new TeamProjectSettingsDTO
			                             {
				                             LocalDB = strLocalDB,
				                             RefDB = strRefDB
			                             };

			return new TeamProjectSettings(dto);
		}
		#endregion
	}
}