using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using v;

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
			//Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 800;
		}

		// Called when the game should load its content
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			var ship = Content.Load<Model>("Content/ship__cv1");
			var ground = Content.Load<Model>("Content/ground");
			//models.Add(new CModel(ship, new Vector3(0, 400, 0), Vector3.Zero, new Vector3(0.4f), GraphicsDevice));
			models.Add(new CModel(ground, Vector3.Zero, Vector3.Zero, Vector3.Zero, GraphicsDevice));

			camera = new ChaseCamera(new Vector3(0, 400, 1500), new Vector3(0, 200, 0), Vector3.Zero, GraphicsDevice);
			lastMouseState = Mouse.GetState();

			base.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			updateModel(gameTime);
			updateCamera(gameTime);

			base.Update(gameTime);
		}

		private void updateModel(GameTime gameTime)
		{
		}

		private void updateCamera(GameTime gameTime)
		{
			// Move the camera to the new model's position and orientation
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (CModel model in models)
			{
				if (camera.BoundingVolumeIsInView(model.BoundingSphere))
				{
					model.Draw(camera.View, camera.Projection);
				}
			}

			base.Draw(gameTime);
		}
	}
}
