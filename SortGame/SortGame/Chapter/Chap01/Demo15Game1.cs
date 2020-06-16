using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
	public class Demo15Game1 : MyBaseGame
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

			models.Add(new CModel(ship, new Vector3(0, 400, 0), Vector3.Zero, new Vector3(0.4f), GraphicsDevice));
			models.Add(new CModel(ground, Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice, false));

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
			KeyboardState keyState = Keyboard.GetState();

			Vector3 rotChange = Vector3.Zero;

			// Determine on which axes the ship should be rotated on, if any
			if (keyState.IsKeyDown(Keys.W))
				rotChange += Vector3.Right;
			if (keyState.IsKeyDown(Keys.S))
				rotChange += Vector3.Left;
			if (keyState.IsKeyDown(Keys.A))
				rotChange += Vector3.Up;
			if (keyState.IsKeyDown(Keys.D))
				rotChange += Vector3.Down;

			models[0].Rotation += rotChange * 0.025f;

			// If space isn't down, the ship shouldn't move
			if (!keyState.IsKeyDown(Keys.Space))
			{
				return;
			}

			// Determine what direction to move in
			Matrix rotation = Matrix.CreateFromYawPitchRoll(models[0].Rotation.Y, models[0].Rotation.X, models[0].Rotation.Z);

			// Move in the direction dictated by our rotation matrix
			models[0].Position += Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
		}

		private void updateCamera(GameTime gameTime)
		{
			// Move the camera to the new model's position and orientation
			((ChaseCamera)camera).Move(models[0].Position, models[0].Rotation);

			// Update the camera
			camera.Update();
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
