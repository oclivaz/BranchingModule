using System.Collections.Generic;

namespace BranchingModule.Logic
{
	public interface IBranchConventionRegistry
	{
		void Register(IBranchConvention convention);
		IBranchConvention GetConvention(BranchInfo branch);
		IEnumerable<IBranchConvention> GetAllConventions();
	}
}