using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Xna.Framework;

namespace MyGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
	        int index = Convert.ToInt32(ConfigurationManager.AppSettings["game"]);
	        IDictionary<int, Game> dictionary = GetGameDictionary();
	        Game game = null;
	        try
	        {
				game = dictionary[index];
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

		private static IDictionary<int, Game> GetGameDictionary()
		{
			IDictionary<int, Game> dictionary = new Dictionary<int, Game>();

			// Chap01
			dictionary.Add(11, new Demo11Game1());
			dictionary.Add(12, new Demo12Game1());
			dictionary.Add(13, new Demo13Game1());
			dictionary.Add(14, new Demo14Game1());
			dictionary.Add(15, new Demo15Game1());

			// Chap02
			dictionary.Add(21, new Demo21Game1());

			// Chap03
			dictionary.Add(31, new Demo31Game1());
			dictionary.Add(32, new Demo32Game1());
			dictionary.Add(33, new Demo33Game1());
			dictionary.Add(34, new Demo34Game1());

			// Chap04
			dictionary.Add(41, new Demo41Game1());
			dictionary.Add(42, new Demo42Game1());
			dictionary.Add(43, new Demo43Game1());

			// Chap05
			dictionary.Add(51, new Demo51Game1());
			dictionary.Add(52, new Demo52Game1());
			dictionary.Add(53, new Demo53Game1());
			dictionary.Add(54, new Demo54Game1());
			dictionary.Add(55, new Demo55Game1());

			return dictionary;
		}
    }
#endif
}

