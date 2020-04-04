using Microsoft.Xna.Framework;

namespace MyGame
{
	public interface IRenderable
	{
		void Draw(Matrix view, Matrix projection, Vector3 cameraPosition);
		void SetClipPlane(Vector4? plane);
	}
}
