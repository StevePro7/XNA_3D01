using System;
using Foundation;
using UIKit;

namespace MyGame.IOS02
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		//private static Game1 game;

		internal static void RunGame()
		{
			var game = new Demo62Game1();
			game.Run();
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
