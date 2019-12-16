using System;
using Microsoft.Xna.Framework;
using WindowsGame.Common.Static;
using WindowsGame.Common.TheGame;
using WindowsGame.Master;

namespace WindowsGame.Common
{
	public static class MyGame
	{
		public static void Construct(IGameManager manager)
		{
			Manager = manager;
		}

		public static void Initialize()
		{
			Manager.Logger.Initialize();
			Manager.RandomManager.Initialize();

			Manager.ConfigManager.Initialize();
			Manager.ConfigManager.LoadContent();

			Manager.ContentManager.Initialize();
			Manager.ContentManager.LoadContentSplash();

			Manager.DeviceManager.Initialize();
			Manager.InputManager.Initialize();

			Manager.ResolutionManager.Initialize();
			Manager.ScreenManager.Initialize();
			Manager.ThreadManager.Initialize();
		}

		public static void LoadContent()
		{
			Engine.Game.IsFixedTimeStep = Constants.IsFixedTimeStep;
			Engine.Game.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Constants.FramesPerSecond);
			Engine.Game.IsMouseVisible = Constants.IsMouseVisible;
			Manager.ResolutionManager.LoadContent(Constants.IsFullScreen, Constants.ScreenWide, Constants.ScreenHigh, Constants.UseExposed, Constants.ExposeWide, Constants.ExposeHigh);
		}

		public static void LoadContentAsync()
		{
			Manager.ContentManager.LoadContent();

			Manager.ScreenManager.LoadContent();

			GC.Collect();
		}

		public static void UnloadContent()
		{
			Engine.Game.Content.Unload();
		}

		public static void Update(GameTime gameTime)
		{
			Manager.InputManager.Update(gameTime);

#if WINDOWS
			if (Manager.ConfigManager.GlobalConfigData.QuitsToExit)
			{
				Boolean escape = Manager.InputManager.Escape();
				if (escape)
				{
					Engine.Game.Exit();
					return;
				}
			}
#endif

			Manager.ScreenManager.Update(gameTime);
		}

		public static void Draw()
		{
			Manager.ScreenManager.Draw();
		}

		public static void OnActivated()
		{
		}
		public static void OnDeactivated()
		{
#if ANDROID
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
			System.Environment.Exit(0);
#endif
		}

		public static IGameManager Manager { get; private set; }
	}
}