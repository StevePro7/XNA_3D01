using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame.IOS
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model model;
        Matrix[] modelTransforms;
        Texture2D image;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Content/ship");
            image = Content.Load<Texture2D>("Content/ship_tex");

            modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //SExit();
            }

            base.Update(gameTime);
        }

        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix view = Matrix.CreateLookAt(
                new Vector3(200, 300, 900),
                new Vector3(0, 50, 0),
                Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), GraphicsDevice.Viewport.AspectRatio,
                0.1f, 10000.0f);

            // Calculate the starting world matrix
            Matrix baseWorld = Matrix.CreateScale(0.4f) *
                Matrix.CreateRotationY(MathHelper.ToRadians(180));

            foreach (ModelMesh mesh in model.Meshes)
            {
                // Calculate each mesh's world matrix
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                    * baseWorld;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    BasicEffect e = (BasicEffect)part.Effect;

                    // Set the world, view, and projection 
                    // matrices to the effect
                    e.World = localWorld;
                    e.View = view;
                    e.Projection = projection;
                    //http://community.monogame.net/t/problem-with-3d-objects-textures/2474/12
                    e.Texture = image;
                    //e.TextureEnabled = false;
                    e.EnableDefaultLighting();
                }

                // Draw the mesh
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
