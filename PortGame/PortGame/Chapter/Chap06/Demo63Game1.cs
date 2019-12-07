using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Demo63Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        ParticleSystem ps;
        ParticleSystem smoke;
        Random r = new Random();

		public Demo63Game1()
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

            models.Add(new CModel(Content.Load<Model>("ground"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

            Effect effect = Content.Load<Effect>("LightingEffect");

            LightingMaterial mat = new LightingMaterial();
            mat.SpecularColor = Color.Black.ToVector3();

            models[0].SetModelEffect(effect, true);
            models[0].Material = mat;
            
            camera = new FreeCamera(new Vector3(0, 700, 3000),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(5),
                GraphicsDevice);

            ps = new ParticleSystem(GraphicsDevice, Content, Content.Load<Texture2D>("fire"), 
                400, new Vector2(400), 1, Vector3.Zero, 0.5f);

            smoke = new ParticleSystem(GraphicsDevice, Content, Content.Load<Texture2D>("smoke"),
                400, new Vector2(800), 6, new Vector3(500, 0, 0), 5f);

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            updateCamera(gameTime);

            // Generate a direction within 15 degrees of (0, 1, 0)
            Vector3 offset = new Vector3(MathHelper.ToRadians(10.0f));
            Vector3 randAngle = Vector3.Up + randVec3(-offset, offset);
                
            // Generate a position between (-400, 0, -400) and (400, 0, 400)
            Vector3 randPosition = randVec3(new Vector3(-400), new Vector3(400));

            // Generate a speed between 600 and 900
            float randSpeed = (float)r.NextDouble() * 300 + 600;

            ps.AddParticle(randPosition, randAngle, randSpeed);
            ps.Update();

            smoke.AddParticle(randPosition + new Vector3(0, 1200, 0), randAngle, randSpeed);
            smoke.Update();

            base.Update(gameTime);
        }

        // Returns a random Vector3 between min and max
        Vector3 randVec3(Vector3 min, Vector3 max)
        {
            return new Vector3(
                min.X + (float)r.NextDouble() * (max.X - min.X),
                min.Y + (float)r.NextDouble() * (max.Y - min.Y),
                min.Z + (float)r.NextDouble() * (max.Z - min.Z));
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

            ps.Draw(camera.View, camera.Projection, 
                ((FreeCamera)camera).Up, ((FreeCamera)camera).Right);

            smoke.Draw(camera.View, camera.Projection,
                ((FreeCamera)camera).Up, ((FreeCamera)camera).Right);
            
            base.Draw(gameTime);
        }
    }
}
