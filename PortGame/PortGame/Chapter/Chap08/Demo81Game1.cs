using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Demo81Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

		public Demo81Game1()
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

            camera = new FreeCamera(new Vector3(100, 65, 100),
                MathHelper.ToRadians(45),
                MathHelper.ToRadians(-30),
                GraphicsDevice);

            Effect lit = Content.Load<Effect>("Content/LightingEffect");
            Effect normal = Content.Load<Effect>("Content/NormalMapEffect");

            LightingMaterial marble = new LightingMaterial();
            marble.SpecularColor = Color.White.ToVector3();

            LightingMaterial steel = new LightingMaterial();
            steel.SpecularColor = Color.Gray.ToVector3();

            NormalMapMaterial brick = new NormalMapMaterial(
                Content.Load<Texture2D>("Content/brick_normal_map"));

            NormalMapMaterial wood = new NormalMapMaterial(
                Content.Load<Texture2D>("Content/wood_normal"));

            CModel model = new CModel(Content.Load<Model>("Content/multimesh__cv1"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice);

            model.SetMeshEffect("Box01", normal, true);
            model.SetMeshMaterial("Box01", wood);

            model.SetMeshEffect("Pyramid01", normal, true);
            model.SetMeshMaterial("Pyramid01", brick);

            model.SetMeshEffect("Sphere01", lit, true);
            model.SetMeshMaterial("Sphere01", marble);

            model.SetMeshEffect("Plane01", lit, true);
            model.SetMeshMaterial("Plane01", steel);

            models.Add(model);

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

            // Move 4 units per millisecond, independent of frame rate
            translation *= 0.5f * 
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
