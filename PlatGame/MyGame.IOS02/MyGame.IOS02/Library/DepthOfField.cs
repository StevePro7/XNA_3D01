using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MyGame
{
    public class DepthOfField : PostProcessor
    {
        // Depth map and un-blurred render of scene. The blurred render
        // will be set as the Input value
        public Texture2D DepthMap;
        public Texture2D Unblurred;

        public DepthOfField(GraphicsDevice graphicsDevice, ContentManager Content)
            : base(Content.Load<Effect>("Content/DepthOfField"), graphicsDevice)
        {
        }

        public override void Draw()
        {
            // Set the two textures above to the second and third
            // texture slots
            graphicsDevice.Textures[1] = Unblurred;
            graphicsDevice.Textures[2] = DepthMap;

            // Set the samplers for all three textures to PointClamp
            // so we can sample pixel values directly
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            graphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

            base.Draw();
        }
    }
}
