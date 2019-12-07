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
            IDictionary<int, Game> dictionary = new Dictionary<int, Game>();

            // Chap01
            dictionary.Add(11, new Demo11Game1());

            return dictionary;
        }
    }

    
#endif
    }
