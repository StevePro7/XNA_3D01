using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame.Common.Interfaces;
using WindowsGame.Common.Static;
using WindowsGame.Master;

namespace WindowsGame.Common.Screens
{
	public class TitleScreen : BaseScreen, IScreen
	{
		public override void Initialize()
		{
			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();
		}

		public ScreenType Update(GameTime gameTime)
		{
			return ScreenType.Title;
		}

		public override void Draw()
		{
            //Engine.SpriteBatch.Draw(Assets.SplashTexture, BannerPosition, Color.White);
        }

	}
}