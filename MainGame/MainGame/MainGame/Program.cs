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
			dictionary.Add(1, new Demo11Game1());
			dictionary.Add(2, new Demo12Game1());
			dictionary.Add(3, new Demo13Game1());
			dictionary.Add(4, new Demo14Game1());
			dictionary.Add(5, new Demo15Game1());
			return dictionary;
		}
    }
#endif
}

