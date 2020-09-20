using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xna.Framework;

namespace MyGame
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
			int index = Convert.ToInt32(ConfigurationManager.AppSettings["game"]);
			IDictionary<int, Game> dictionary = GetGameDictionary();

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
				if(null != game)
				{
					game.Dispose();
				}
			}
		}

		private static IDictionary<int, Game> GetGameDictionary()
		{
			IDictionary<int, Game> dictionary = new Dictionary<int, Game>
			{
				// Chap01
				{ 11, new Demo11Game1() },
				{ 12, new Demo12Game1() },
				{ 13, new Demo13Game1() },
				{ 14, new Demo14Game1() },
				{ 15, new Demo15Game1() },

				// Chap02
				{ 21, new Demo21Game1() },
				
				// Chap03
				{ 31, new Demo31Game1() },
				{ 32, new Demo32Game1() },
				{ 33, new Demo33Game1() },
				{ 34, new Demo34Game1() },
				
				// Chap04
				{ 41, new Demo41Game1() },
				{ 42, new Demo42Game1() },
				{ 43, new Demo43Game1() },
				
				// Chap05
				{ 51, new Demo51Game1() },
				{ 52, new Demo52Game1() },
				{ 53, new Demo53Game1() },
				{ 54, new Demo54Game1() },
				{ 55, new Demo55Game1() },
				
				// Chap06
				{ 61, new Demo61Game1() },
				{ 62, new Demo62Game1() },
				{ 63, new Demo63Game1() },

				// Chap07
				{ 71, new Demo71Game1() },
				{ 72, new Demo72Game1() },
				{ 73, new Demo73Game1() },
				{ 74, new Demo74Game1() },
				
				// Chap08
				{ 81, new Demo81Game1() },
				{ 82, new Demo82Game1() },
				{ 83, new Demo83Game1() },
				{ 84, new Demo84Game1() },
				{ 85, new Demo85Game1() },
				/*
				// Chap09
				{ 91, new Demo91Game1() },
				{ 92, new Demo92Game1() },
				{ 93, new Demo93Game1() },
				{ 94, new Demo94Game1() },
				{ 95, new Demo95Game1() }
				*/
			};

			return dictionary;
		}

	}
#endif
}
