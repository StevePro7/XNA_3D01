using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;

namespace MyGame
{
	public class Demo83Game1 : MyBaseGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		List<CModel> models = new List<CModel>();
		Camera camera;

		MouseState lastMouseState;

		RenderCapture renderCapture;
		PostProcessor postprocessor;

		public Demo83Game1()
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

			camera = new FreeCamera(new Vector3(1000, 650, 1000),
				MathHelper.ToRadians(45),
				MathHelper.ToRadians(-30),
				GraphicsDevice);

			Effect effect = Content.Load<Effect>("Content/LightingEffect");
			LightingMaterial mat = new LightingMaterial();

			for (int z = -1; z <= 1; z++)
				for (int x = -1; x <= 1; x++)
				{
					CModel model = new CModel(Content.Load<Model>("Content/teapot__cv1"),
						new Vector3(x * 500, 0, z * 500), Vector3.Zero,
						Vector3.One * 50, GraphicsDevice);

					model.SetModelEffect(effect, true);
					model.SetModelMaterial(mat);

					model.Model.Meshes[0].MeshParts[0].Effect.Parameters["BasicTexture"].SetValue(
						Content.Load<Texture2D>("Content/brick_texture_map"));
					model.Model.Meshes[0].MeshParts[0].Effect.Parameters["TextureEnabled"].SetValue(true);

					models.Add(model);
				}

			CModel ground = new CModel(Content.Load<Model>("Content/ground"),
				Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice, false);

			//ground.SetModelEffect(effect, true);
			//ground.SetModelMaterial(mat);

			models.Add(ground);

			renderCapture = new RenderCapture(GraphicsDevice);
			postprocessor = new GaussianBlur(GraphicsDevice, Content, 2);
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
			renderCapture.Begin();

			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (CModel model in models)
				if (camera.BoundingVolumeIsInView(model.BoundingSphere))
					model.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

			renderCapture.End();

			GraphicsDevice.Clear(Color.Black);

			postprocessor.Input = renderCapture.GetTexture();
			postprocessor.Draw();

			base.Draw(gameTime);
		}

	}
}
