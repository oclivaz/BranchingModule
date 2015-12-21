using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace BranchingModule.Cmdlets
{
	internal class DynamicParameter<T> : IDynamicParameter
	{
		#region Properties
		public T Value
		{
			get
			{
				return this.CmdLet.MyInvocation.BoundParameters.ContainsKey(this.Name)
					? (T) this.CmdLet.MyInvocation.BoundParameters[this.Name]
					: default(T);
			}
		}

		private BranchingModulePSCmdletBase CmdLet { get; set; }
		private string Name { get; set; }
		private bool Mandatory { get; set; }
		private int Posistion { get; set; }
		private string[] AllowedValues { get; set; }
		#endregion

		#region Constructors
		public DynamicParameter(BranchingModulePSCmdletBase cmdLet, string strName, bool bMandatory, int nPosition)
		{
			this.CmdLet = cmdLet;
			this.Name = strName;
			this.Mandatory = bMandatory;
			this.Posistion = nPosition;

			cmdLet.AddDynamicParameter(this);
		}

		public DynamicParameter(BranchingModulePSCmdletBase cmdLet, string strName, bool bMandatory, int nPosition, string[] allowedValues)
			: this(cmdLet, strName, bMandatory, nPosition)
		{
			this.AllowedValues = allowedValues;
		}
		#endregion

		#region Publics
		public void AddRuntimeDefinedParameterTo(RuntimeDefinedParameterDictionary parameters)
		{
			ParameterAttribute paramAttribute = new ParameterAttribute { Mandatory = this.Mandatory, Position = this.Posistion };
			Collection<Attribute> collection = new Collection<Attribute> { paramAttribute };

			if(this.AllowedValues != null && this.AllowedValues.Any()) collection.Add(new ValidateSetAttribute(this.AllowedValues));

			RuntimeDefinedParameter param = new RuntimeDefinedParameter(this.Name, typeof(T), collection);
			parameters.Add(this.Name, param);
		}

		public static implicit operator T(DynamicParameter<T> param)
		{
			return param.Value;
		}
		#endregion
	}
}