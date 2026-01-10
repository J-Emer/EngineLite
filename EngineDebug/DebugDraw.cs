using System;
using System.Collections.Generic;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.EngineDebug
{
    public static class DebugDraw
    {


        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness = 1)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(
                                AssetLoader.GetPixel(),
                                start,
                                null,
                                color,
                                angle,
                                Vector2.Zero,
                                new Vector2(length, thickness),
                                SpriteEffects.None,
                                0
                            );
        }

    }
}
