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
			dictionary.Add(1, new Load01Game1());
			dictionary.Add(2, new Load02Game1());
			dictionary.Add(3, new Load03Game1());
			dictionary.Add(4, new Load04Game1());
			dictionary.Add(5, new Load05Game1());
			return dictionary;
		}
    }
#endif
}

