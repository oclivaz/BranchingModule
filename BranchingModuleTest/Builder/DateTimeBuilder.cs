using System;

namespace BranchingModuleTest.Builder
{
	internal class DateTimeBuilder
	{
		#region Properties
		private DateTime Date { get; set; }
		#endregion

		#region Constructors
		public DateTimeBuilder(int nDay, int nMont, int nYear)
		{
			this.Date = new DateTime(nYear, nMont, nDay);
		}
		#endregion

		#region Publics
		public DateTime At(int nHour, int nMinute)
		{
			return new DateTime(this.Date.Year, this.Date.Month, this.Date.Day, nHour, nMinute, 0);
		}
		#endregion
	}
}