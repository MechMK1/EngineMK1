using System;
using System.Collections.Generic;
using System.Linq;

namespace MechMK1.EngineMK1
{
	/// <summary>
	/// Class which contains all meta-functionality about running the game, but no game-related code.
	/// </summary>
	public static class GameMaster
	{
		#region Events
		/// <summary>
		/// Fires when the Main Loop is called. Use this to register your commands!
		/// </summary>
		public static event EventHandler Initialize;
		/// <summary>
		/// Fires when the Main Loop is about to run. Use this to check if your commands were added successfully!
		/// </summary>
		public static event EventHandler InitializeFinished;
		/// <summary>
		/// Fires when a command was parsed, but before being executed.
		/// </summary>
		public static event EventHandler<CommandWithArgs> PreCommandExecute;
		/// <summary>
		/// Fires after a command was executed.
		/// </summary>
		public static event EventHandler<CommandWithArgs> PostCommandExecute;
		/// <summary>
		/// Fires when a command was parsed, but not found in the command list.
		/// </summary>
		public static event EventHandler<CommandWithArgs> CommandNotFound;
		/// <summary>
		/// Fires after the Main Loop has finished and shows the command which caused the exit.
		/// </summary>
		public static event EventHandler<CommandWithArgs> Exit;
		#endregion

		/// <summary>
		/// Holds all registered commands and corresponding actions to call on each command.
		/// </summary>
		internal static Dictionary<string, Action<string[]>> commandList = new Dictionary<string, Action<string[]>>();

		/// <summary>
		/// Set this to false to cause the Main Loop to quit (and causing the game to exit).
		/// </summary>
		internal static bool _continue = true;

		/// <summary>
		/// Starts the game and prompts the user for input.
		/// </summary>
		public static void MainLoop()
		{
			//Bootstrap Initialization
			Initialize += EventHandlers.InitializeHandler;

			OnInit();		//Fires the Initialize Event
			OnInitFinish(); //Fires the InitializeFinished Event

			CommandWithArgs result = null;
			while (_continue)
			{
				result = ReadCommandAndExecute(); //Reads, parses and executes a command
			}

			OnExit(result); //Fires the Exit Event
		}

		/// <summary>
		/// Registers a command for use in the Main Loop
		/// </summary>
		/// <param name="command">Name of the command. Must not contain a space</param>
		/// <param name="callback">Action which is executed when the command is being called.</param>
		/// <returns>Success of the operation</returns>
		public static bool RegisterCommand(string command, Action<string[]> callback)
		{
			//Do not allow spaces in commands
			if (command.Contains(' '))
			{
				Logging.Logger.Error(string.Format("Tried to register command '{0}', command contains illegal character!", command));
				throw new ArgumentException("command must not contain a space", "command");
			}

			//Warn users about double-registered commands. Do not overwrite the command and return false instead.
			if (GameMaster.commandList.Keys.Contains<string>(command))
			{
				Logging.Logger.Warning(string.Format("Tried to register command '{0}', but it was already registered!", command));
				return false;
			}

			//Add command to the list and log the success
			GameMaster.commandList.Add(command, callback);
			Logging.Logger.Info(string.Format("Registered command '{0}' successfully! (Action is: {1}.{2})'", command, callback.Method.ReflectedType.FullName, callback.Method.Name));
			return true;
		}

		/// <summary>
		/// Read user input, parse it and execute if possible
		/// </summary>
		/// <returns>The parsed command and optional arguments</returns>
		private static CommandWithArgs ReadCommandAndExecute()
		{
			Console.Write("> "); //Show a nifty little prompt because we're 1337
			
			string input = Console.ReadLine(); //Read a line typed by the user
			string [] split = input.Split(new char [] {' '});	//Split it by whitespaces
																//TODO Allow using whitespaces between " "
			string command = split.First<string>(); //Use the first string as command
			string[] args = split.Skip(1).ToArray<string>(); //Use the rest as arguments

			// If no arguments were found, set args to null instead of an empty array.
			if (args.Count<string>() == 0)
			{
				args = null;
			}

			//Put the command and arguments into a neat object.
			CommandWithArgs result = new CommandWithArgs() { Command = command, Args = args };
			OnPreCommandExecute(result); //Fire the PreCommandExecute event

			//Check if the command was registered
			if (GameMaster.commandList.Keys.Contains(result.Command))
			{ //If so...
				//Get the corresponding action
				Action<string[]> action = GameMaster.commandList[result.Command];
				//Execute the action
				action.Invoke(result.Args);
				//And fire the PostCommandExecute event
				OnPostCommandExecute(result);
			}
			else
			{//If not...
				//Fire the CommandNotFound event
				OnCommandNotFound(result);
			}
			
			//Return the command the user typed
			return result;
		}

		#region EventCallbacks
		/// <summary>
		/// Called when the Main Loop exits gracefully (aka. no crash)
		/// </summary>
		/// <param name="result">Last command issued by the user before exit</param>
		private static void OnExit(CommandWithArgs result)
		{
			if (Exit != null)
				Exit(null, result);
		}

		/// <summary>
		/// Called when the Main Loop is being called.
		/// </summary>
		private static void OnInit()
		{
			if (Initialize != null)
				Initialize(null, EventArgs.Empty);
		}

		/// <summary>
		/// Called when Initialization is complete.
		/// </summary>
		private static void OnInitFinish()
		{
			if (InitializeFinished != null)
				InitializeFinished(null, EventArgs.Empty);
		}

		/// <summary>
		/// Called after a command was parsed, but before execution
		/// </summary>
		/// <param name="result">Parsed command</param>
		private static void OnPreCommandExecute(CommandWithArgs result)
		{
			if (PreCommandExecute != null)
				PreCommandExecute(null, result);
		}

		/// <summary>
		/// Called after a command was parsed and executed
		/// </summary>
		/// <param name="result">Parsed command</param>
		private static void OnPostCommandExecute(CommandWithArgs result)
		{
			if (PostCommandExecute != null)
				PostCommandExecute(null, result);
		}

		/// <summary>
		/// Called when a command was parsed, but was not found in the commandList
		/// </summary>
		/// <param name="result">Parsed command</param>
		private static void OnCommandNotFound(CommandWithArgs result)
		{
			if (CommandNotFound != null)
				CommandNotFound(null, result);
		}
		#endregion

	}
}
