using MechMK1.SimpLog;

namespace MechMK1.EngineMK1
{
	/// <summary>
	/// Helper Class which initializes one static logger and makes it accessible to the namespace
	/// </summary>
	internal static class Logging
	{
		private static Logger _logger = new FileLogger("EngineMK1.log",
#if DEBUG
			LogLevel.Debug
#else
			LogLevel.Info
#endif
);
		internal static Logger Logger { get { return _logger; } }
	}
}
