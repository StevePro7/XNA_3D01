using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MyGame
{
	public class RaceTrack
	{
		// List of control points
		List<Vector2> positions;

		// Vertex and index buffers
		VertexBuffer vb;
		IndexBuffer ib;
		int nVertices, nIndices;

		// Rendering variables
		GraphicsDevice graphicsDevice;
		BasicEffect effect;
		Texture2D texture;

		// Total length of the track
		float trackLength;

		public RaceTrack(List<Vector2> positions, int nDivisions, float trackWidth,
			int textureRepetitions, GraphicsDevice graphicsDevice, ContentManager content)
		{
			this.graphicsDevice = graphicsDevice;
			this.positions = interpolatePositions(positions, nDivisions);

			effect = new BasicEffect(graphicsDevice);
			texture = content.Load<Texture2D>("track");

			createBuffers(trackWidth, textureRepetitions);
		}

		// Adds the given number of positions between the control points specified,
		// to subdivide/smooth the path
		List<Vector2> interpolatePositions(List<Vector2> positions, int nDivisions)
		{
			// Create a new list of positions
			List<Vector2> newPositions = new List<Vector2>();

			// Between each control point...
			for (int i = 0; i < positions.Count - 1; i++)
			{
				// Add the control point to the new list
				newPositions.Add(positions[i]);

				// Add the specified number of interpolated points
				for (int j = 0; j < nDivisions; j++)
				{
					// Determine how far to interpolate
					float amt = (float)(j + 1) / (float)(nDivisions + 2);

					// Find the position based on catmull-rom interpolation
					Vector2 interp = catmullRomV2(
						positions[wrapIndex(i - 1, positions.Count - 1)],
						positions[i],
						positions[wrapIndex(i + 1, positions.Count - 1)],
						positions[wrapIndex(i + 2, positions.Count - 1)], amt);

					// Add the new position to the new list
					newPositions.Add(interp);
				}
			}

			return newPositions;
		}

		// Wraps a number around 0 and the "max" value
		int wrapIndex(int value, int max)
		{
			while (value > max)
				value -= max;
			while (value < 0)
				value += max;

			return value;
		}

		// Performs a Catmull-Rom interpolation for each component of a Vector2 based
		// on the given control points and interpolation distance
		Vector2 catmullRomV2(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float amount)
		{
			return new Vector2(MathHelper.CatmullRom(v1.X, v2.X, v3.X, v4.X, amount),
				MathHelper.CatmullRom(v1.Y, v2.Y, v3.Y, v4.Y, amount));
		}

		void createBuffers(float trackWidth, int textureRepetitions)
		{
			VertexPositionNormalTexture[] vertices = createVertices(trackWidth,
				textureRepetitions);

			// Create vertex buffer and set data
			vb = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture),
				vertices.Length, BufferUsage.WriteOnly);
			vb.SetData<VertexPositionNormalTexture>(vertices);

			int[] indices = createIndices();

			// Reach compatibility requires 16 bit indices (short instead of int)
			if (graphicsDevice.GraphicsProfile == GraphicsProfile.Reach)
			{
				short[] indices16 = new short[indices.Length];

				for (int i = 0; i < indices.Length; i++)
					indices16[i] = (short)indices[i];

				// Create index buffer and set data
				ib = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits,
					indices.Length, BufferUsage.WriteOnly);
				ib.SetData<short>(indices16);
			}
			else
			{
				// Create index buffer and set data
				ib = new IndexBuffer(graphicsDevice, IndexElementSize.ThirtyTwoBits,
					indices.Length, BufferUsage.WriteOnly);
				ib.SetData<int>(indices);
			}
		}

		VertexPositionNormalTexture[] createVertices(float trackWidth, int textureRepetitions)
		{
			// Create 2 vertices for each track point
			nVertices = positions.Count * 2;
			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[nVertices];

			int j = 0;
			trackLength = 0;

			for (int i = 0; i < positions.Count; i++)
			{
				// Find the index of the next position
				int next = wrapIndex(i + 1, positions.Count - 1);

				// Find the current and next positions on the path
				Vector3 position = new Vector3(positions[i].X, 0, positions[i].Y);
				Vector3 nextPosition = new Vector3(positions[next].X, 0, positions[next].Y);

				// Find the vector between the current and next position
				Vector3 forward = nextPosition - position;
				float length = forward.Length();
				forward.Normalize();

				// Find the side vector based on the forward and up vectors
				Vector3 side = -Vector3.Cross(forward, Vector3.Up) * trackWidth;

				// Create a vertex to the left and right of the current position
				vertices[j++] = new VertexPositionNormalTexture(position - side,
					Vector3.Up, new Vector2(0, trackLength));
				vertices[j++] = new VertexPositionNormalTexture(position + side,
					Vector3.Up, new Vector2(1, trackLength));

				trackLength += length;
			}

			// Attach the end vertices to the beginning to close the loop
			vertices[vertices.Length - 1].Position = vertices[1].Position;
			vertices[vertices.Length - 2].Position = vertices[0].Position;

			// For each vertex...
			for (int i = 0; i < vertices.Length; i++)
			{
				// Bring the UV's Y coordinate back to the [0, 1] range
				vertices[i].TextureCoordinate.Y /= trackLength;

				// Tile the texture along the track
				vertices[i].TextureCoordinate.Y *= textureRepetitions;
			}

			return vertices;
		}

		int[] createIndices()
		{
			// Create indices
			nIndices = (positions.Count - 1) * 6;
			int[] indices = new int[nIndices];

			int j = 0;

			// Create two triangles between every position
			for (int i = 0; i < positions.Count - 1; i++)
			{
				int i0 = i * 2;

				indices[j++] = i0;
				indices[j++] = i0 + 1;
				indices[j++] = i0 + 2;
				indices[j++] = i0 + 2;
				indices[j++] = i0 + 1;
				indices[j++] = i0 + 3;
			}

			return indices;
		}

		// Returns the position on the track the given distance from the start,
		// and the forward direction at that point
		public Vector2 TracePath(float distance, out Vector2 direction)
		{
			// Remove extra laps
			while (distance > trackLength)
				distance -= trackLength;

			int i = 0;

			while (true)
			{
				// Find the index of the next and last position
				int last = wrapIndex(i - 1, positions.Count - 1);
				int next = wrapIndex(i + 1, positions.Count - 1);

				// Find the distance between this position and the next
				direction = positions[next] - positions[i];
				float length = direction.Length();

				// If the length remaining is greater than the distance to
				// the next position, keep looping. Otherwise, the
				// final position is somewhere between the current and next
				// position in the list
				if (length < distance)
				{
					distance -= length;
					i++;
					continue;
				}

				// Find the direction from the last position to the current position
				Vector2 lastDirection = positions[i] - positions[last];
				lastDirection.Normalize();
				direction.Normalize();

				// Determine how far the position is between the current and next
				// positions in the list
				float amt = distance / length;

				// Interpolate the last and current direction and current and 
				// next position to find final direction and position
				direction = Vector2.Lerp(lastDirection, direction, amt);
				return Vector2.Lerp(positions[i], positions[next], amt);
			}
		}

		public void Draw(Matrix View, Matrix Projection)
		{
			// Set effect parameters
			effect.World = Matrix.Identity;
			effect.View = View;
			effect.Projection = Projection;
			effect.Texture = texture;
			effect.TextureEnabled = true;

			// Set the vertex and index buffers to the graphics device
			graphicsDevice.SetVertexBuffer(vb);
			graphicsDevice.Indices = ib;

			// Apply the effect
			effect.CurrentTechnique.Passes[0].Apply();

			// Draw the list of triangles
			PrimitiveType primitiveType = PrimitiveType.TriangleList;
			int baseVertex = 0;
			//int minVertexIndex = 0;
			int numVertices = nVertices;
			int startIndex = 0;
			int primitiveCount = nIndices / 3;

			//graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, nVertices, 0, nIndices / 3);
			//graphicsDevice.DrawIndexedPrimitives(primitiveType, baseVertex, minVertexIndex, numVertices, startIndex, primitiveCount);
			//stevepro warning
			graphicsDevice.DrawIndexedPrimitives(primitiveType, baseVertex, startIndex, primitiveCount);
		}
	}
}
