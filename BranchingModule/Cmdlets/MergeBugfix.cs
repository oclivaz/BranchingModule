﻿using System;
using System.Management.Automation;
using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	[Cmdlet("Merge", "Bugfix")]
	public class MergeBugfix : PSCmdlet, ITextOutputListener
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
		public string Changeset { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 2
			)]
		public string Targetbranches { get; set; }

		[Parameter(
			Mandatory = false,
			Position = 3
			)]
		public SwitchParameter NoCheckIn { get; set; }
		#endregion

		#region Protecteds
		protected override void ProcessRecord()
		{
			IDependencyInjectionFactory factory = new DependencyInjectionFactory();
			MergeBugfixController controller = factory.Get<MergeBugfixController>();

			ITextOutputService textOutputService = factory.Get<ITextOutputService>();
			textOutputService.RegisterListener(this);

			try
			{
				controller.MergeBugfix(this.Teamproject, this.Changeset, this.Targetbranches.Split(','), this.NoCheckIn);
			}
			catch(Exception ex)
			{
				WriteObject(ex.StackTrace);
				throw;
			}
		}
		#endregion
	}
}