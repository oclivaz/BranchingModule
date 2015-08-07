using System.Management.Automation;
using BranchingModule.Logic;
using Ninject;

namespace BranchingModule.Cmdlets

{
	[Cmdlet(VerbsCommon.Add, "Mapping")]
	public class AddMapping : PSCmdlet
	{
		#region Properties
		[Parameter(
			Mandatory = true,
			Position = 0
			)]
		public string Teamproject { get; set; }

		[Parameter(
			Mandatory = true,
			Position = 1
			)]
		public string Branch { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IKernel kernel = new StandardKernel(new InjectionModule());
			AddMappingController controller = kernel.Get<AddMappingController>();

			controller.Process(this.Teamproject, this.Branch);
		}
		#endregion
	}
}