﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
	public class Demo91Game1 : MyBaseGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<CModel> models = new List<CModel>();
		Camera camera;

		MouseState lastMouseState;

		ObjectAnimation anim;


		public Demo91Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			//Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
		}

		// Called when the game should load its content
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			camera = new FreeCamera(new Vector3(650, 650, 650),
				MathHelper.ToRadians(45),
				MathHelper.ToRadians(-30),
				GraphicsDevice);

			models.Add(new CModel(Content.Load<Model>("Content/ship__cv1"),
				Vector3.Zero, Vector3.Zero, new Vector3(0.25f), GraphicsDevice));

			anim = new ObjectAnimation(new Vector3(0, -150, 0), new Vector3(0, 150, 0), Vector3.Zero,
				new Vector3(0, -MathHelper.TwoPi, 0), TimeSpan.FromSeconds(10), true);

			lastMouseState = Mouse.GetState();
		}

		// Called when the game should update itself
		protected override void Update(GameTime gameTime)
		{
			updateCamera(gameTime);

			anim.Update(gameTime.ElapsedGameTime);

			models[0].Position = anim.Position;
			models[0].Rotation = anim.Rotation;

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
			translation *= 0.5f *
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
			{
				if (camera.BoundingVolumeIsInView(model.BoundingSphere))
				{
					model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
				}
			}

			base.Draw(gameTime);
		}
	}
}
