using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class MyBaseGame : Microsoft.Xna.Framework.Game
    {
        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}
#endif
			base.Update(gameTime);
        }
    }
}
