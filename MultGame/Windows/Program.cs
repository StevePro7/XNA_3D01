using Microsoft.Xna.Framework;
using MyGame;
using System;
using System.Collections.Generic;
using System.Configuration;

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
			int index = 21;
			//int index = Convert.ToInt32(ConfigurationManager.AppSettings["game"]);

			GameDictionary obj = new GameDictionary();
			IDictionary<int, Game> dictionary = obj.GetGameDictionary();

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
