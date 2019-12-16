using System;
using WindowsGame.Master.Interfaces;
using WindowsGame.Common.Managers;

namespace WindowsGame.Common.TheGame
{
	public interface IGameManager
	{
		IConfigManager ConfigManager { get; }
		IContentManager ContentManager { get; }
		IDeviceManager DeviceManager { get; }
		IInputManager InputManager { get; }
		IRandomManager RandomManager { get; }
		IResolutionManager ResolutionManager { get; }
		IScreenManager ScreenManager { get; }
        ITextManager TextManager { get; }
        IThreadManager ThreadManager { get; }
        IFileManager FileManager { get; }
		ILogger Logger { get; }
	}

	public class GameManager : IGameManager
	{
		public GameManager
		(
			IConfigManager configManager,
			IContentManager contentManager,
			IDeviceManager deviceManager,
			IInputManager inputManager,
			IRandomManager randomManager,
			IResolutionManager resolutionManager,
			IScreenManager screenManager,
            ITextManager textManager,
            IThreadManager threadManager,
            IFileManager fileManager,
			ILogger logger
		)
		{
			ConfigManager = configManager;
			ContentManager = contentManager;
			DeviceManager = deviceManager;
			InputManager = inputManager;
			RandomManager = randomManager;
			ResolutionManager = resolutionManager;
			ScreenManager = screenManager;
            TextManager = textManager;
            ThreadManager = threadManager;
			FileManager = fileManager;
			Logger = logger;
		}

		public IConfigManager ConfigManager { get; private set; }
		public IContentManager ContentManager { get; private set; }
		public IDeviceManager DeviceManager { get; private set; }
		public IInputManager InputManager { get; private set; }
		public IRandomManager RandomManager { get; private set; }
		public IResolutionManager ResolutionManager { get; private set; }
		public IScreenManager ScreenManager { get; private set; }
        public ITextManager TextManager { get; private set; }
        public IThreadManager ThreadManager { get; private set; }
		public IFileManager FileManager { get; private set; }
		public ILogger Logger { get; private set; }
	}
}