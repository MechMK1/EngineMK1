namespace MechMK1.EngineMK1
{
	
	/// <summary>
	/// Entity to hold a command and the accompanying arguments
	/// </summary>
	public class CommandWithArgs
	{
		/// <summary>
		/// The command which was input by the user
		/// </summary>
		public string Command { get; set; }
		/// <summary>
		/// Optional arguments to the command.
		/// </summary>
		public string[] Args { get; set; }
	}
}
