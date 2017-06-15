using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace CLSLogger
{
	[DebuggerDisplay("{ToString()}")]
	public class LogMessage
	{
		#region Private Variables

		String _hostMachine;
		String _application;

		#endregion

		#region Properties
		public Int32 MsgVersion { get; set; }
		public DateTime EventTime { get; set; }
		public DateTime LogTime { get; set; }
		public Int32 SequenceNo { get; set; }
		public LogLevel Level { get; set; }
		public Int32 Area { get; set; }
		public Int32 Office { get; set; }
		public Int32 Seat { get; set; }
		public Int32 DiscardedMessages { get; set; }
		public Int32 HeartBeat { get; set; }
		public Int32 Flags { get; set; }
		public String Data { get; set; }

		#endregion

		#region Constructors

		public LogMessage () : this ( LogLevel.LVL_SUCCESS, String.Empty ) { }

		public LogMessage ( LogLevel level, String data ) : this( 0, DateTime.Now, DateTime.Now, 0, level, 0, 0, 0, 0, 0, 0, data ) { }

		public LogMessage
		(
			Int32 version,
			DateTime eventTime, 
			DateTime logTime, 
			Int32 sequenceNo, 
			LogLevel level, 
			Int32 area, 
			Int32 office , 
			Int32 seat, 
			Int32 discardedMessages, 
			Int32 heartBeat, 
			Int32 flags, 
			String data 
		)
		{
			MsgVersion = version;
			EventTime = eventTime;
			LogTime = logTime;
			SequenceNo =  sequenceNo; 
			Level = level;
			Area = area;
			Office = office;
			Seat = seat;
			_hostMachine = Environment.MachineName;
			_application = AppDomain.CurrentDomain.FriendlyName;

			_application.Remove(_application.IndexOf('.'));
			_application = (_application.Length > 8)? _application.Remove(8) : _application;
			_application = (_application.Length < 8) ? _application.PadRight(8) : _application;

			DiscardedMessages = discardedMessages;
			HeartBeat = heartBeat;
			Flags = flags;
			Data = data;
		}

		#endregion

		#region Object Overrides

		public override string ToString()
		{
			return String.Format
			(
				"{0} {1} {2} {3} {4} {5}/{6}/{7} {8} {9} {10} {11} {12} {13}"
				, this.MsgVersion // 0
				, this.EventTime.GetLogDateTime() // 1
				, this.LogTime.GetLogDateTime() // 2
				, this.SequenceNo.ToString().PadLeft(3, '0') // 3
				, ((Int32)this.Level).ToString().PadLeft(3, '0') // 4
				, this.Area.ToString().PadLeft(2, '0') // 5
				, this.Office.ToString().PadLeft(4, '0') // 6
				, this.Seat.ToString().PadLeft(3, '0') // 7
				, this._hostMachine.PadRight(15, ' ') 
				, this._application.PadRight(8, ' ')
				, this.DiscardedMessages.ToString().PadLeft(3, '0') // 10
				, this.HeartBeat.ToString().PadLeft(6, '0') // 11
				, this.Flags.ToString().PadLeft(2, '0') // 12
				, this.Data // 13
			);
		}

		#endregion

	}
}
