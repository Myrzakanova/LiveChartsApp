using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsApp
{
    class StatsCalculator
    {

		public static (double winChange, double macChange, double linuxChange) GetWeeklyChanges(DateTime selectedDate)
		{
			var currentData = DataBase.GetEntryByDate(selectedDate);
			var previousData = DataBase.GetEntryByDate(selectedDate.AddDays(-7));

			if (currentData == null || previousData == null)
				return (0, 0, 0);

			double CalculateChange(int current, int previous)
			{
				if (previous == 0) return current > 0 ? 100 : 0;
				return ((current - previous) / (double)previous) * 100;
			}

			return (
				CalculateChange(currentData.Windows, previousData.Windows),
				CalculateChange(currentData.Mac, previousData.Mac),
				CalculateChange(currentData.Linux, previousData.Linux)
			);
		}
	}
}
