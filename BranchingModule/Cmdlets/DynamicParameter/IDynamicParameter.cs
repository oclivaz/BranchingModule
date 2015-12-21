using System.Management.Automation;

namespace BranchingModule.Cmdlets
{
	internal interface IDynamicParameter
	{
		void AddRuntimeDefinedParameterTo(RuntimeDefinedParameterDictionary parameters);
	}
}
