using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MyGame
{
    public class Demo12Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();

		public Demo12Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                {
                    Vector3 position = new Vector3(
                        -600 + x * 600, -400 + y * 400, 0);

                    models.Add(new CModel(Content.Load<Model>("ship__cv1"),
                        position,
                        new Vector3(0, MathHelper.ToRadians(90) * (y * 3 + x), 0),
                        new Vector3(0.25f), GraphicsDevice));
                }
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix view = Matrix.CreateLookAt(
                new Vector3(0, 300, 2000),
                new Vector3(0, 0, 0),
                Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio,
                0.1f, 10000.0f);

            foreach (CModel model in models)
                model.Draw(view, projection);

            base.Draw(gameTime);
        }


    }
}
