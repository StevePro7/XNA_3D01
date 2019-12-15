using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Demo85Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        RenderCapture renderCapture;
        RenderCapture glowCapture;
        Effect glowEffect;
        GaussianBlur blur;

        public Demo85Game1()
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

            camera = new FreeCamera(new Vector3(1000, 650, 1000),
                MathHelper.ToRadians(45),
                MathHelper.ToRadians(-30),
                GraphicsDevice);

            Effect effect = Content.Load<Effect>("Content/LightingEffect");
            LightingMaterial mat = new LightingMaterial();

            for (int z = -1; z <= 1; z++)
                for (int x = -1; x <= 1; x++)
                {
                    CModel model = new CModel(Content.Load<Model>("Content/glow_teapot__cv1"),
                        new Vector3(x * 500, 0, z * 500), Vector3.Zero,
                        Vector3.One * 5, GraphicsDevice);

                    model.SetModelEffect(effect, true);
                    model.SetModelMaterial(mat);

                    models.Add(model);
                }

            CModel ground = new CModel(Content.Load<Model>("Content/glow_plane__cv1"),
                Vector3.Zero, Vector3.Zero, Vector3.One * 6, GraphicsDevice);

            ground.SetModelEffect(effect, true);
            ground.SetModelMaterial(mat);

            models.Add(ground);

            renderCapture = new RenderCapture(GraphicsDevice);
            glowCapture = new RenderCapture(GraphicsDevice);

            glowEffect = Content.Load<Effect>("Content/GlowEffect");
            glowEffect.Parameters["GlowTexture"].SetValue(
                Content.Load<Texture2D>("Content/glow_map"));

            blur = new GaussianBlur(GraphicsDevice, Content, 4);

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
            // Begin capturing the glow render
            glowCapture.Begin();

            GraphicsDevice.Clear(Color.Black);

            // Draw all models with the glow effect/texture applied, reverting
            // the effect when finished
            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                {
                    model.CacheEffects();
                    model.SetModelEffect(glowEffect, false);
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
                    model.RestoreEffects();
                }

            // Finish capturing the glow
            glowCapture.End();

            // Draw the scene regularly into the other RenderCapture
            renderCapture.Begin();

            GraphicsDevice.Clear(Color.Black);

            // Draw all models
            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            // Finish capturing
            renderCapture.End();

            // Blur the glow render back into the glow RenderCapture
            blur.Input = glowCapture.GetTexture();
            blur.ResultCapture = glowCapture;
            blur.Draw();

            GraphicsDevice.Clear(Color.Black);

            // Draw the blurred glow render over the normal render additively
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.Draw(renderCapture.GetTexture(), Vector2.Zero, Color.White);
            spriteBatch.Draw(glowCapture.GetTexture(), Vector2.Zero, Color.White);
            spriteBatch.End();

            // Clean up after the SpriteBatch
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            base.Draw(gameTime);
        }
    }
}
