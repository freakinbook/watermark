using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Watermark3
{
	interface ILogger
	{
		void Log(string message);
	}

	class Logger : ILogger
	{
		public Logger()
		{
			Trace.Listeners.Clear();
			Trace.Listeners.Add(new TextWriterTraceListener("report.log","logger"));
			Trace.Listeners["logger"].TraceOutputOptions = TraceOptions.DateTime;
		}

		public void Log(string message)
		{
			Trace.TraceInformation(message);
			Trace.Flush();
		}
	}
}
