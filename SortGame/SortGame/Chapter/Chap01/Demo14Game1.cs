using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using v;
using System.Collections.Generic;

namespace MyGame
{
	public class Demo14Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<CModel> models = new List<CModel>();
		Camera camera;

		MouseState lastMouseState;

		public Demo14Game1()
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

			Model model = Content.Load<Model>("Content/ship__cv1");
			models.Add(new CModel(model, Vector3.Zero, Vector3.Zero, new Vector3(0.6f), GraphicsDevice));

			camera = new ArcBallCamera(Vector3.Zero, 0, 0, 0, MathHelper.PiOver2, 1200, 1000, 2000, GraphicsDevice);

			lastMouseState = Mouse.GetState();
		}

		// Called when the game should update itself
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			updateCamera(gameTime);
		}

		private void updateCamera(GameTime gameTime)
		{
			// Get the new keyboard and mouse state
			MouseState mouseState = Mouse.GetState();
			KeyboardState keyState = Keyboard.GetState();

			// Determine how must the camera should turn
			float deltaX = (float)lastMouseState.X - (float)mouseState.X;
			float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

			// Rotate the camera
			((ArcBallCamera)camera).Rotate(deltaX * 0.01f, deltaY * 0.01f);

			// Calculate scroll wheel movement
			float scrollDelta = (float)lastMouseState.ScrollWheelValue - (float)mouseState.ScrollWheelValue;

			// Move the camera
			((ArcBallCamera)camera).Move(scrollDelta);

			// Update the camera
			camera.Update();

			// Update the mouse state
			lastMouseState = mouseState;
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach(CModel model in models)
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
