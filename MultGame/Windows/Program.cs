using Microsoft.Xna.Framework;
using MyGame;
using System;
using System.Collections.Generic;

namespace Windows
{
#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			GameDictionary obj = new GameDictionary();
			IDictionary<int, Game> dictionary = obj.GetGameDictionary();

			//int index = Convert.ToInt32(ConfigurationManager.AppSettings["game"]);
			int index = 11;

			Game game = null;
			try
			{
				game = dictionary[index];
				game.Window.Title = $"Demo{index}Game";
				game.IsMouseVisible = true;
				game.Run();
			}
			finally
			{
				if (null != game)
				{
					game.Dispose();
				}
			}
		}
	}
#endif
}
