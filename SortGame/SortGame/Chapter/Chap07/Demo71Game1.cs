using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
	public class Demo71Game1 : MyBaseGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<CModel> models = new List<CModel>();
		Camera camera;

		MouseState lastMouseState;

		Terrain terrain;

		public Demo71Game1()
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

			camera = new FreeCamera(new Vector3(8000, 6000, 8000),
				MathHelper.ToRadians(45),
				MathHelper.ToRadians(-30),
				GraphicsDevice);

			terrain = new Terrain(Content.Load<Texture2D>("Content/terrain"), 30, 4800,
				Content.Load<Texture2D>("Content/grass"), 6, new Vector3(1, -1, 0),
				GraphicsDevice, Content);

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
			translation *= 10 *
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

			terrain.Draw(camera.View, camera.Projection);

			base.Draw(gameTime);
		}
	}
}
