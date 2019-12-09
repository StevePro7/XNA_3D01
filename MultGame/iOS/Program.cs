﻿using Foundation;
using Microsoft.Xna.Framework;
using MyGame;
using System.Collections.Generic;
using UIKit;

namespace iOS
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		private static Game1 game;

		internal static void RunGame()
		{
			int index = 11;

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
