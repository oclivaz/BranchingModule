using System.Collections.Generic;
using System.Linq;

namespace BranchingModule.Logic
{
	internal class BranchConventionFactory : IBranchConventionFactory
	{
		#region Properties
		private ISet<IBranchConvention> BranchConventions { get; set; }
		#endregion

		#region Constructors
		public BranchConventionFactory()
		{
			this.BranchConventions = new HashSet<IBranchConvention>();
		}
		#endregion

		#region Publics
		public void RegisterBranchConvention(IBranchConvention convention)
		{
			this.BranchConventions.Add(convention);
		}

		public IBranchConvention GetConvention(BranchInfo branch)
		{
			var matchingConventions = from branchConvention in this.BranchConventions
			                          where branchConvention.BranchnameFollowsConvention(branch.Name)
			                          select branchConvention;

			return matchingConventions.Single();
		}

		public IEnumerable<IBranchConvention> GetAllConventions()
		{
			return this.BranchConventions;
		}
		#endregion
	}
}