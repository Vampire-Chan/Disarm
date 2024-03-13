using System;
using System.IO;
using System.Text;

namespace Logs;

public class Logger
{
	[Flags]
	private enum LogLevel
	{
		TRACE = 0,
		INFO = 1,
		DEBUG = 2,
		WARNING = 3,
		ERROR = 4,
		FATAL = 5
	}

	private readonly object fileLock = new object();

	private readonly string datetimeFormat;

	private readonly string logFilename;

	public Logger(string name)
	{
		datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
		logFilename = name;
		string text = logFilename + " is created.";
		if (!File.Exists(logFilename))
		{
			WriteLine(DateTime.Now.ToString(datetimeFormat) + " " + text);
		}
	}

	public void Debug(string text)
	{
		WriteFormattedLog(LogLevel.DEBUG, text);
	}

	public void Error(string text)
	{
		WriteFormattedLog(LogLevel.ERROR, text);
	}

	public void Fatal(string text)
	{
		WriteFormattedLog(LogLevel.FATAL, text);
	}

	public void Info(string text)
	{
		WriteFormattedLog(LogLevel.INFO, text);
	}

	public void Trace(string text)
	{
		WriteFormattedLog(LogLevel.TRACE, text);
	}

	public void Warning(string text)
	{
		WriteFormattedLog(LogLevel.WARNING, text);
	}

	private void WriteLine(string text, bool append = false)
	{
		try
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			lock (fileLock)
			{
				using StreamWriter streamWriter = new StreamWriter(logFilename, append, Encoding.UTF8);
				streamWriter.WriteLine(text);
			}
		}
		catch
		{
			throw;
		}
	}

	private void WriteFormattedLog(LogLevel level, string text)
	{
		WriteLine(level switch
		{
			LogLevel.TRACE => DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ", 
			LogLevel.INFO => DateTime.Now.ToString(datetimeFormat) + " [INFO]    ", 
			LogLevel.DEBUG => DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ", 
			LogLevel.WARNING => DateTime.Now.ToString(datetimeFormat) + " [WARNING] ", 
			LogLevel.ERROR => DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ", 
			LogLevel.FATAL => DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ", 
			_ => "", 
		} + text, append: true);
	}
}
