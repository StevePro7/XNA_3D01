using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Demo95Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        RaceTrack track;
        SkySphere sky;

		public Demo95Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new FreeCamera(new Vector3(1000, 650, 1000),
                MathHelper.ToRadians(45),
                MathHelper.ToRadians(-30),
                GraphicsDevice);

            camera = new ChaseCamera(new Vector3(0, 250, 700), new Vector3(0, 250, 0), 
                new Vector3(0, MathHelper.Pi, 0), GraphicsDevice);

            lastMouseState = Mouse.GetState();

            List<Vector2> trackPositions = new List<Vector2>() {
                new Vector2(-4000, 0),
                new Vector2(-4000, -4000),
                new Vector2(0, -4000),
                new Vector2(4000, -4000),
                new Vector2(4000, -2000),
                new Vector2(0, -2000),
                new Vector2(-1000, 0),
                new Vector2(0, 2000),
                new Vector2(4000, 2000),
                new Vector2(4000, 4000),
                new Vector2(0, 4000),
                new Vector2(-4000, 4000),
                new Vector2(-4000, 0)
            };

            track = new RaceTrack(trackPositions, 25, 300, 30, GraphicsDevice, Content);

            models.Add(new CModel(Content.Load<Model>("Ground"), new Vector3(0, -10f, 0), Vector3.Zero, Vector3.One * 2, GraphicsDevice));
            models.Add(new CModel(Content.Load<Model>("car"), Vector3.Zero, 
                Vector3.Zero, Vector3.One, GraphicsDevice));

            foreach (CModel model in models)
                foreach(ModelMesh mesh in model.Model.Meshes)
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.LightingEnabled = true;
                        effect.PreferPerPixelLighting = true;
                        effect.AmbientLightColor = new Vector3(.4f);
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(.6f);
                        effect.DirectionalLight0.Direction = new Vector3(1, -1, -1);
                        effect.DirectionalLight0.Direction.Normalize();
                        effect.DirectionalLight0.SpecularColor = Color.Black.ToVector3();
                    }

            sky = new SkySphere(Content, GraphicsDevice, Content.Load<TextureCube>("clouds"));
        }

        float distance = 0;
        float speed = 0;
        
        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            // Update the car speed for acceleration, braking, and friction
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                speed += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                speed -= 2500 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                speed -= 1500 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Limit the speed min/max
            speed = MathHelper.Clamp(speed, 0, 2000);

            // Increase the distance based on speed
            distance += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Get position and direction of car from track class
            Vector2 direction;
            Vector2 trackPosition = track.TracePath(distance, out direction);

            // Convert direction vector to angle
            float rotation = (float)Math.Acos(direction.Y > 0 ? -direction.X : direction.X);

            if (direction.Y > 0)
                rotation += MathHelper.Pi;

            rotation += MathHelper.PiOver2;

            // Move and rotate car accordingly
            models[1].Position = new Vector3(trackPosition.X, 20, trackPosition.Y);
            models[1].Rotation = new Vector3(0, rotation, 0);

            ((ChaseCamera)camera).Move(models[1].Position, models[1].Rotation);
            ((ChaseCamera)camera).Update();

            base.Update(gameTime);
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sky.Draw(camera.View, camera.Projection, ((ChaseCamera)camera).Position);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((ChaseCamera)camera).Position);

            track.Draw(camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}
