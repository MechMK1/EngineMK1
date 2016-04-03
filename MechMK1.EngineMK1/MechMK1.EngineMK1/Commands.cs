using System;

namespace MechMK1.EngineMK1
{
	/// <summary>
	/// Class which holds all commands supplied to the Game Master
	/// </summary>
	static class DefaultCommands
	{
		#region Commands
		/// <summary>
		/// Is being called when user inputs an empty line. Does nothing.
		/// </summary>
		/// <param name="args">Ignored</param>
		internal static void DoNothing(string [] args)
		{

		}

		/// <summary>
		/// Stops the main loop and causes the game to quit.
		/// </summary>
		/// <param name="args">Ignored</param>
		internal static void Exit(string [] args)
		{
			GameMaster._continue = false;
		}

		/// <summary>
		/// Debug Method used to test display of arguments
		/// </summary>
		/// <param name="args">Arguments to be displayed</param>
		internal static void ShowArgs(string [] args)
		{
			Console.WriteLine("Hello! I am a command, and I can show you some arguments!");
			if (args == null || args.Length == 0)
			{
				Console.WriteLine("Oh, it looks like I don't have arguments. Bummer!");
			}
			else
			{
				Console.Write("My arguments are as follows:");
				foreach (var arg in args)
				{
					Console.Write(" '{0}'", arg);
				}
				Console.WriteLine();
				Console.WriteLine("Aren't they beautiful?");
			}
		}
		#endregion
	}
}
