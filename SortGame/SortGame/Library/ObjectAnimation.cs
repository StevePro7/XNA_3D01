using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
	public class ObjectAnimation
	{
		Vector3 startPosition, endPosition, startRotation, endRotation;
		TimeSpan duration;
		bool loop;

		TimeSpan elapsedTime = TimeSpan.FromSeconds(0);

		public Vector3 Position { get; private set; }
		public Vector3 Rotation { get; private set; }

		public ObjectAnimation(Vector3 StartPosition, Vector3 EndPosition,
			Vector3 StartRotation, Vector3 EndRotation, TimeSpan Duration, bool Loop)
		{
			this.startPosition = StartPosition;
			this.endPosition = EndPosition;
			this.startRotation = StartRotation;
			this.endRotation = EndRotation;
			this.duration = Duration;
			this.loop = Loop;
			Position = startPosition;
			Rotation = startRotation;
		}

		public void Update(TimeSpan Elapsed)
		{
			// Update the time
			this.elapsedTime += Elapsed;

			// Determine how far along hte duration value we are (0 to 1)
			float amt = (float)elapsedTime.TotalSeconds / (float)duration.TotalSeconds;

			if (loop)
				while (amt > 1)     // Wrap the time if we are looping
					amt -= 1;
			else
			{
				// Clamp to the end value if we are not
				amt = MathHelper.Clamp(amt, 0, 1);
			}

			// Update the current position and rotation
			Position = Vector3.Lerp(startPosition, endPosition, amt);
			Rotation = Vector3.Lerp(startRotation, endRotation, amt);
		}
	}

	public class ObjectAnimationFrame
	{
		public Vector3 Position { get; private set; }
		public Vector3 Rotation { get; private set; }
		public TimeSpan Time { get; private set; }

		public ObjectAnimationFrame(Vector3 Position, Vector3 Rotation, TimeSpan Time)
		{
			this.Position = Position;
			this.Rotation = Rotation;
			this.Time = Time;
		}
	}

	public class KeyFrameObjectAnimation
	{
		List<ObjectAnimationFrame> frames = new List<ObjectAnimationFrame>();
		bool loop;
		TimeSpan elapsedTime = TimeSpan.FromSeconds(0);

		public Vector3 Position { get; private set; }
		public Vector3 Rotation { get; private set; }

		public KeyFrameObjectAnimation(List<ObjectAnimationFrame> Frames, bool loop)
		{
			this.frames = Frames;
			this.loop = loop;
			Position = Frames[0].Position;
			Rotation = Frames[0].Rotation;
		}

		public void Update(TimeSpan Elapsed)
		{
			// Update the time
			this.elapsedTime += Elapsed;

			TimeSpan totalTime = elapsedTime;
			TimeSpan end = frames[frames.Count - 1].Time;

			if (loop)       // loop around the total time if necessary
				while (totalTime > end)
					totalTime -= end;
			else
			{
				// Otherwise clamp to the end values
				Position = frames[frames.Count - 1].Position;
				Rotation = frames[frames.Count - 1].Rotation;
			}

			int i = 0;

			// Find the index of the current frame
			while (frames[i + 1].Time < totalTime)
				i++;

			// Find the time since the beginning of this frame
			totalTime -= frames[i].Time;

			// Find how far we are between the current and next frame (0 to 1)
			float amt = (float)((totalTime.TotalSeconds) /
				(frames[i + 1].Time - frames[i].Time).TotalSeconds);

			// Interpolate position and rotation values between frames
			Position = Vector3.Lerp(frames[i].Position, frames[i + 1].Position, amt);
			Rotation = Vector3.Lerp(frames[i].Rotation, frames[i + 1].Rotation, amt);
		}
	}

}
