using System.Collections.Generic;

namespace BranchingModule.Logic
{
	public interface IBranchConventionFactory
	{
		void RegisterBranchConvention(IBranchConvention convention);
		IBranchConvention GetConvention(BranchInfo branch);
		IEnumerable<IBranchConvention> GetAllConventions();
	}
}