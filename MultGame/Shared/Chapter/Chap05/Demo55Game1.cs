using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class Demo55Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;
        SkySphere sky;

        MouseState lastMouseState;

        Water water;

        public Demo55Game1()
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

            models.Add(new CModel(Content.Load<Model>("teapot"),
                Vector3.Zero, Vector3.Zero, Vector3.One * 50, GraphicsDevice));

            Effect cubeMapEffect = Content.Load<Effect>("CubeMapReflect");
            CubeMapReflectMaterial cubeMat = new CubeMapReflectMaterial(
                Content.Load<TextureCube>("clouds"));

            models[0].SetModelEffect(cubeMapEffect, false);
            models[0].Material = cubeMat;

            Plane clip = new Plane(Vector3.Up, 0);

            sky = new SkySphere(Content, GraphicsDevice,
                Content.Load<TextureCube>("clouds"));

            water = new Water(Content, GraphicsDevice,
                new Vector3(0, 0, 0), new Vector2(8000, 8000));

            water.Objects.Add(sky);
            water.Objects.Add(models[0]);

            //water.Objects.Add(models[1]);

            camera = new FreeCamera(new Vector3(0, 400, 1400),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(0),
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
            water.PreDraw(camera, gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            sky.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
            water.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection,
                        ((FreeCamera)camera).Position);

            base.Draw(gameTime);
        }
    }
}
