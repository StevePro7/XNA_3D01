using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MyGame
{
	public class Demo12Game1 : MyBaseGame
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		IList<CModel> models = new List<CModel>();

		public Demo12Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			//Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1200;
			graphics.PreferredBackBufferHeight = 800;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Model model = Content.Load<Model>("Content/ship__cv1");

			for(int y = 0; y < 3; y++)
			{
				for(int x = 0; x < 3; x++)
				{
					Vector3 position = new Vector3(600 + x * 600, -400 + y * 400, 0);
					Vector3 rotation = new Vector3(0, MathHelper.ToRadians(90) * (y * 3 + x), 0);
					Vector3 scale = new Vector3(0.25f);

					//CModel model = new CModel(model, position, rotation, scale, GraphicsDevice);
					//models.Add(model);
				}
			}
			
			base.LoadContent();
		}
	}
}
