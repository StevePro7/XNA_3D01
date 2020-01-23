using Microsoft.Xna.Framework;

namespace MyGame
{
	public interface IRenderable
	{
		void Draw(Matrix view, Matrix projection, Matrix cameraPosition);
		void SetClipPlane(Vector4? plane);
	}
}
