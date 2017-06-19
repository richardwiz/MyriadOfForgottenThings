// -----------------------------------------------------------------------
// <copyright file="CLSLogManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CLSLogger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using ClsErrorLib;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
	public class CLSLogManager
	{
        #region Private Variables

        private LogHelper _log;

        #endregion

		#region Properties

		public String LogFile { get; set; }

		#endregion
		#region Constructors

		public CLSLogManager() : this(String.Empty) { }

		public CLSLogManager(String logFile)
		{
            _log = new LogHelper("Nijord");
        }

		#endregion

		#region Class Methods

		public Boolean Log(LogMessage msg)
		{
			try
			{
                _log.Log((ErrorLog.LEVEL)msg.Level, (ErrorLog.FLAG)msg.Flags, msg.Data);
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex);
			}
			
			return true;
		}
		#endregion

		#region Object Overrides

		public override string ToString()
		{
			return LogFile;
		}

		#endregion
	}
}
