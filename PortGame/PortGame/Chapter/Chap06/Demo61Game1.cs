using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
    public class Demo61Game1 : MyBaseGame
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<CModel> models = new List<CModel>();
        Camera camera;

        MouseState lastMouseState;

        BillboardSystem trees;
        BillboardSystem clouds;

		public Demo61Game1()
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

            models.Add(new CModel(Content.Load<Model>("Content/grass_ground"),
                Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice, false));

            Effect effect = Content.Load<Effect>("Content/LightingEffect");

            LightingMaterial mat = new LightingMaterial();
            mat.SpecularColor = Color.Black.ToVector3();

            //models[0].SetModelEffect(effect, true);
            //models[0].Material = mat;
            
            camera = new FreeCamera(new Vector3(0, 700, 3000),
                MathHelper.ToRadians(0),
                MathHelper.ToRadians(5),
                GraphicsDevice);

            // Generate random tree positions
            Random r = new Random();
            Vector3[] positions = new Vector3[100];

            for (int i = 0; i < positions.Length; i++)
                positions[i] = new Vector3(
                    (float)r.NextDouble() * 20000 - 10000, 
                    400, 
                    (float)r.NextDouble() * 20000 - 10000
                );

            trees = new BillboardSystem(GraphicsDevice, Content, 
                Content.Load<Texture2D>("Content/tree_billboard"), new Vector2(800), 
                positions);

            trees.Mode = BillboardSystem.BillboardMode.Cylindrical;

            Vector3[] cloudPositions = new Vector3[350];

            for (int i = 0; i < cloudPositions.Length; i++)
            {
                cloudPositions[i] = new Vector3(
                    r.Next(-6000, 6000),
                    r.Next(2000, 3000),
                    r.Next(-6000, 6000));
            }

            clouds = new BillboardSystem(GraphicsDevice, Content,
                Content.Load<Texture2D>("Content/cloud2"), new Vector2(1000),
                cloudPositions);

            clouds.EnsureOcclusion = false;

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

            foreach (CModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            trees.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Up, ((FreeCamera)camera).Right);
            clouds.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Up, ((FreeCamera)camera).Right);
            
            base.Draw(gameTime);
        }
    }
}
