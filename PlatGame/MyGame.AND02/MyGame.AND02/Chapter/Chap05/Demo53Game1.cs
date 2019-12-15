using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Demo53Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;
        SkySphere sky;

        MouseState lastMouseState;

        public Demo53Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(new CModel(Content.Load<Model>("Content/brick_wall__cv1"),
                new Vector3(0, 200, 0), Vector3.Zero, Vector3.One, GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("Content/Ground"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice, false));

            Effect lightingEffect = Content.Load<Effect>("Content/LightingEffect");
            LightingMaterial lightingMat = new LightingMaterial();
            
            Effect normalMapEffect = Content.Load<Effect>("Content/NormalMapEffect");
            NormalMapMaterial normalMat = new NormalMapMaterial(
                Content.Load<Texture2D>("Content/brick_normal_map"));

            lightingMat.LightDirection = new Vector3(.5f, .5f, 1);
            lightingMat.LightColor = Vector3.One;

            normalMat.LightDirection = new Vector3(.5f, .5f, 1);
            normalMat.LightColor = Vector3.One;

            models[0].SetModelEffect(lightingEffect, true);
            //models[1].SetModelEffect(normalMapEffect, true);

            models[0].Material = lightingMat;
            //models[1].Material = normalMat;

            camera = new FreeCamera(new Vector3(0, 400, 1400),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(0),
                GraphicsDevice);

            sky = new SkySphere(Content, GraphicsDevice,
                Content.Load<TextureCube>("Content/clouds"));

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sky.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            base.Draw(gameTime);
        }
    }
}
