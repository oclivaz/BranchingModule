﻿using BranchingModule.Logic;

namespace BranchingModule.Cmdlets
{
	internal class DynamicParameterFactory
	{
		#region Publics
		public static DynamicParameter<string> CreateTeamProjectParameter(BranchingModulePSCmdletBase cmdLet, int nPosition)
		{
			ISettings settings = ControllerFactory.Get<ISettings>();

			return new DynamicParameter<string>(cmdLet, "TeamProject", true, nPosition, settings.SupportedTeamprojects);
		}

		public static DynamicParameter<string> CreateBranchParameter(BranchingModulePSCmdletBase cmdLet, int nPosition)
		{
			return new DynamicParameter<string>(cmdLet, "Branch", false, nPosition);
		}

		public static DynamicParameter<bool> CreateMinimalParameter(BranchingModulePSCmdletBase cmdLet, int nPosition)
		{
			return new DynamicParameter<bool>(cmdLet, "Minimal", false, nPosition);
		}

		public static DynamicParameter<bool> CreateOpenSolutionParameter(BranchingModulePSCmdletBase cmdLet, int nPosition)
		{
			return new DynamicParameter<bool>(cmdLet, "Opensolution", false, nPosition);
		}
		#endregion
	}
}