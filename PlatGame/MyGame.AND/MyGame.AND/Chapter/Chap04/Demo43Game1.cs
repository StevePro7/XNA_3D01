using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Demo43Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        PrelightingRenderer renderer;

		public Demo43Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // http://community.monogame.net/t/effect-loading-sharpdx-sharpdxexception-occurred-in-sharpdx-dll/8242/3
            // http://community.monogame.net/t/solved-effect-load-errors-with-latest-build-directx/8741
            GraphicsProfile gp1 = graphics.GraphicsProfile;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            GraphicsProfile gp2 = graphics.GraphicsProfile;
            //Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(new CModel(Content.Load<Model>("Content/teapot__cv1"),
                Vector3.Zero, Vector3.Zero, new Vector3(60), 
                GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("Content/ground"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice, false));

            Effect effect = Content.Load<Effect>("Content/VSM");

            models[0].SetModelEffect(effect, true);
            //models[1].SetModelEffect(effect, true);

            camera = new FreeCamera(new Vector3(0, 2200, -200),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(-90),
                GraphicsDevice);

            renderer = new PrelightingRenderer(GraphicsDevice, Content);
            renderer.Models = models;
            renderer.Camera = camera;
            renderer.Lights = new List<PPPointLight>() {
                new PPPointLight(new Vector3(0, 1000, -1000), Color.White * .85f, 20000),
                new PPPointLight(new Vector3(0, 1000, 1000), Color.White * .85f, 20000),
            };
            renderer.ShadowLightPosition = new Vector3(1500, 1500, 2000);
            renderer.ShadowLightTarget = new Vector3(0, 150, 0);
            renderer.DoShadowMapping = true;
            renderer.ShadowMult = 0.3f;

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
		{
			//if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			//{
			//	Exit();
			//}
            updateCamera(gameTime);

            base.Update(gameTime);
        }

        void updateCamera(GameTime gameTime)
        {
            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            // Rotate the camera
            ((FreeCamera)camera).Rotate(deltaX * .005f, deltaY * .005f);

            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;

            // Move 3 units per millisecond, independent of frame rate
            translation *= 4 * 
                (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Move the camera
            ((FreeCamera)camera).Move(translation);

            // Update the camera
            camera.Update();

            // Update the mouse state
            lastMouseState = mouseState;
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw(false);

            GraphicsDevice.Clear(Color.Black);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            base.Draw(gameTime);
        }
    }
}
