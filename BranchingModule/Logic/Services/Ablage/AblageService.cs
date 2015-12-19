namespace BranchingModule.Logic
{
	internal class AblageService : IAblageService
	{
		#region Properties
		public IFileSystemAdapter FileSystemAdapter { get; set; }
		public IConvention Convention { get; set; }
		#endregion

		#region Constructors
		public AblageService(IFileSystemAdapter fileSystemAdapter, IConvention convention)
		{
			this.FileSystemAdapter = fileSystemAdapter;
			this.Convention = convention;
		}
		#endregion

		#region Publics
		public void Reset(BranchInfo branch)
		{
			string strAblagePath = this.Convention.GetAblagePath(branch);

			if(this.FileSystemAdapter.Exists(strAblagePath)) this.FileSystemAdapter.EmptyDirectory(strAblagePath);
			else this.FileSystemAdapter.CreateDirectory(strAblagePath);
		}

		public void Remove(BranchInfo branch)
		{
			string strAblagePath = this.Convention.GetAblagePath(branch);

			if(this.FileSystemAdapter.Exists(strAblagePath)) this.FileSystemAdapter.DeleteDirectory(strAblagePath);
		}
		#endregion
	}
}