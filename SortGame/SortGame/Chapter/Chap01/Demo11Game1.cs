using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	public class Demo11Game1 : MyBaseGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;


		Model model;
		Matrix[] modelTransforms;
		Texture2D image;

		public Demo11Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			//Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1200;
			graphics.PreferredBackBufferHeight = 800;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			model = Content.Load<Model>("Content/ship__cv1");
			image = Content.Load<Texture2D>("Content/ship_tex");

			modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			Matrix view = Matrix.CreateLookAt(
				new Vector3(200, 300, 900),
				new Vector3(0, 50, 0),
				Vector3.Up
				);

			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
				MathHelper.ToRadians(45),
				GraphicsDevice.Viewport.AspectRatio,
				0.1f,
				10000.0f
				);

			// Calculate the starting world matrix
			Matrix baseMatrix = Matrix.CreateScale(0.4f) * Matrix.CreateRotationY(MathHelper.ToRadians(180));

			foreach (ModelMesh mesh in model.Meshes)
			{
				// Calculate each mesh's world matrix
				Matrix localMatrix = modelTransforms[mesh.ParentBone.Index] * baseMatrix;

				foreach (ModelMeshPart part in mesh.MeshParts)
				{
					BasicEffect e = (BasicEffect)part.Effect;

					// Set the world, view and projection matrix to the effect
					e.World = localMatrix;
					e.View = view;
					e.Projection = projection;

					mesh.Draw();
				}
			}

			base.Draw(gameTime);
		}

	}
}
