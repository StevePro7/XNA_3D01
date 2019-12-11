using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Demo33Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

		public Demo33Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // http://community.monogame.net/t/effect-loading-sharpdx-sharpdxexception-occurred-in-sharpdx-dll/8242/3
            // http://community.monogame.net/t/solved-effect-load-errors-with-latest-build-directx/8741
            GraphicsProfile gp1 = graphics.GraphicsProfile;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            GraphicsProfile gp2 = graphics.GraphicsProfile;
            //Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(new CModel(Content.Load<Model>("Content/teapot__cv1"),
                new Vector3(0, 60, 0), Vector3.Zero, new Vector3(60), 
                GraphicsDevice));

            //models.Add(new CModel(Content.Load<Model>("ground"),
            //    Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

            Effect simpleEffect = Content.Load<Effect>("Content/MultiLightingEffect");

            models[0].SetModelEffect(simpleEffect, true);
            //models[1].SetModelEffect(simpleEffect, true);

            MultiLightingMaterial mat = new MultiLightingMaterial();

            BasicEffect effect = new BasicEffect(GraphicsDevice);
            effect.EnableDefaultLighting();

            mat.LightDirection[0] = -effect.DirectionalLight0.Direction;
            mat.LightDirection[1] = -effect.DirectionalLight1.Direction;
            mat.LightDirection[2] = -effect.DirectionalLight2.Direction;

            mat.LightColor = new Vector3[] {
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f) };

            models[0].Material = mat;
            //models[1].Material = mat;

            camera = new FreeCamera(new Vector3(0, 200, 600),
                MathHelper.ToRadians(0), // Turned around 153 degrees
                MathHelper.ToRadians(5), // Pitched up 13 degrees
                GraphicsDevice);

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
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
            GraphicsDevice.Clear(Color.Black);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            base.Draw(gameTime);
        }
    }
}
