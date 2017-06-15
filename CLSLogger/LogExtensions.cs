// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CLSLogger
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public static class LogExtensions
	{
		public static String GetLogDateTime(this DateTime date)
		{
			return date.ToString("yyyyMMddHHmmssfff");
		}
	}
}
