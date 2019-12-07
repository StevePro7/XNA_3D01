using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Demo15Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

		public Demo15Game1()
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

            models.Add(new CModel(Content.Load<Model>("ship"),
                new Vector3(0, 400, 0), Vector3.Zero, new Vector3(0.4f), GraphicsDevice));

            models.Add(new CModel(Content.Load<Model>("ground"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice));

            camera = new ChaseCamera(new Vector3(0, 400, 1500), new Vector3(0, 200, 0),
                new Vector3(0, 0, 0), GraphicsDevice);

            lastMouseState = Mouse.GetState();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            updateModel(gameTime);
            updateCamera(gameTime);

            base.Update(gameTime);
        }

        void updateModel(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            // Determine on which axes the ship should be rotated on, if any
            if (keyState.IsKeyDown(Keys.W))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.S))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            models[0].Rotation += rotChange * .025f;

            // If space isn't down, the ship shouldn't move
            if (!keyState.IsKeyDown(Keys.Space))
                return;

            // Determine what direction to move in
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                models[0].Rotation.Y, models[0].Rotation.X, models[0].Rotation.Z);

            // Move in the direction dictated by our rotation matrix
            models[0].Position += Vector3.Transform(Vector3.Forward, rotation)
                * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
        }

        void updateCamera(GameTime gameTime)
        {
            // Move the camera to the new model's position and orientation
            ((ChaseCamera)camera).Move(models[0].Position, models[0].Rotation);

            // Update the camera
            camera.Update();
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}
