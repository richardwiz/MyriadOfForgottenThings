// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
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
	public enum LogLevel
	{
		   LVL_SUCCESS          = 0,
		   LVL_RDAY             = 1,     
		   LVL_WARNING          = 2,
		   LVL_ERROR            = 3,
		   LVL_FATAL            = 4,
		   LVL_NETWORK          = 5,
		   LVL_DEBUG            = 6,
		   LVL_REPORT           = 7,
		   LVL_INFO             = 8,
		   MIN_LVL_PAGER        = 100,   // min level of pager messages
		   LVL_SPORTALERT       = 101,
		   LVL_TECH_HELP_ALERT  = 102,
		   LVL_MISSINGHEARTBEAT = 150,
		   MAX_LVL_PAGER        = 150,   // max level of pager messages 
		   LVL_MAX              = 255, 
		   //Alarm Levels
		   LVL_ALERT1			= 10,
		   LVL_ALERT2			= 11,
		   LVL_ALERT3			= 12,
		   LVL_ALERT4			= 13,
		   LVL_ALERT5			= 14,
		   LVL_ALERT6			= 15,
		   LVL_ALERT7			= 16,
		   LVL_ALERT8			= 17 
	}
}
