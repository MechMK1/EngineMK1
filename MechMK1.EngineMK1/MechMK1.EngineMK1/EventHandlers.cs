using System;
using MechMK1.SimpLog;

namespace MechMK1.EngineMK1
{
	/// <summary>
	/// Holds all handlers for events triggered by the Game Master
	/// </summary>
	internal static class EventHandlers
	{
		#region EventHandlers
		/// <summary>
		/// Logs which command is about to be executed, before it is actually executed.
		/// Should help when a command crashes the system
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">The command and arguments about to be invoked</param>
		internal static void PreCommandExecuteHandler(object sender, CommandWithArgs e)
		{
			if (e.Args != null)
			{
				Logging.Logger.Debug(string.Format("Command '{0}' is about to be executed with the following arguments:", e.Command));
				foreach (var item in e.Args)
				{
					Logging.Logger.Debug(item);
				}
			}
			else
			{
				Logging.Logger.Debug(string.Format("Command '{0}' is about to be executed without arguments.", e.Command));
			}
		}

		/// <summary>
		/// Logs which command was just executed.
		/// Might help one day. Who knows?
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">The command and arguments which were invoked</param>
		internal static void PostCommandExecuteHandler(object sender, CommandWithArgs e)
		{
			if (e.Args != null)
			{
				Logging.Logger.Debug(string.Format("Command '{0}' was executed with the following arguments:", e.Command));
				foreach (var item in e.Args)
				{
					Logging.Logger.Debug(item);
				}
			}
			else
			{
				Logging.Logger.Debug(string.Format("Command '{0}' was executed without arguments.", e.Command));
			}
		}

		/// <summary>
		/// Handler which registers commands to the Game Master
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">Always empty</param>
		internal static void InitializeHandler(object sender, EventArgs e)
		{
			GameMaster.CommandNotFound += EventHandlers.CommandNotFoundHandler;

			Logging.Logger.Info("Game Master is initializing. Commands are being added now.");
			GameMaster.RegisterCommand("", DefaultCommands.DoNothing);
			GameMaster.RegisterCommand("quit", DefaultCommands.Exit);
			GameMaster.RegisterCommand("exit", DefaultCommands.Exit);
			GameMaster.RegisterCommand("debug", DefaultCommands.ShowArgs);
		}

		/// <summary>
		/// Handler which lists all registered functions after initialization is finished
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">Always empty</param>
		internal static void InitializeFinishedHandler(object sender, EventArgs e)
		{
			Logging.Logger.Info("Game loop is about to run. Game Master knows the following commands:");
			foreach (var item in GameMaster.commandList.Keys)
			{
				Logging.Logger.Info(string.Format("'{0}'", item));
			}
			Logging.Logger.Info("If any commands are missing, they are likely being added later.");
		}

		/// <summary>
		/// Handler which runs once the game loop has finished, showing which command was run last (aka. caused the exit)
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">Always empty</param>
		internal static void ExitHandler(object sender, CommandWithArgs e)
		{
			Logging.Logger.Info(string.Format("Exiting due to command '{0}'", e.Command));
		}

		/// <summary>
		/// Tells the user that a command was not found. This is likely because of a typo, but might show a deeper problem.
		/// </summary>
		/// <param name="sender">Always null</param>
		/// <param name="e">Command which was supplied, but not found</param>
		internal static void CommandNotFoundHandler(object sender, CommandWithArgs e)
		{
			Console.WriteLine("The command '{0}' was not found. Are you missing a registration?", e.Command);
			Logging.Logger.Debug(string.Format("The command '{0}' was not found. Are you missing a registration?", e.Command));
		}
		#endregion
	}
}
