using System;

namespace BranchingModule.Logic
{
	internal class MSConvention : IConvention
	{
		public string GetLocalPath(string strTeamproject, string strBranch)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(strBranch == null) throw new ArgumentNullException("strBranch");

			return string.Format(@"C:\inetpub\wwwroot\{0}_{1}", strTeamproject, strBranch.Replace('.', '_'));
		}

		public string GetServerPath(string strTeamproject, string strBranch)
		{
			if(strTeamproject == null) throw new ArgumentNullException("strTeamproject");
			if(strBranch == null) throw new ArgumentNullException("strBranch");

			return string.Format(@"$/{0}/Release/{1}/Source", strTeamproject, strBranch);
		}
	}
}
