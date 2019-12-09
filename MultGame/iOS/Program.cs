using Microsoft.Xna.Framework;
using MyGame;
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace iOS
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		private static Game1 game;

		internal static void RunGame()
		{
			//game = new Game1();
			//game.Run();

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

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}

		public override void FinishedLaunching(UIApplication app)
		{
			RunGame();
		}
	}
}
