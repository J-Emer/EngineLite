using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Util
{
    public static class ShapeRenderer
    {
        private static Texture2D pixel;
        public static void Init(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color drawColor)
        {
            spriteBatch.Draw(pixel, rectangle, drawColor);
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Point min, Point max, Color drawColor)
        {
            spriteBatch.Draw(pixel, CreateRectangleFromTwoPoints(min, max), drawColor);
        }
        public static void DrawRectangle(SpriteBatch spriteBatch, Vector2[] points, Color color)
        {
            if (points.Length != 4)
                throw new ArgumentException("Rectangle requires exactly 4 points");

            DrawPolygon(spriteBatch, points, color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 1f)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(
                pixel,
                start,
                null,
                color,
                angle,
                new Vector2(0, 0.5f),
                new Vector2(edge.Length(), thickness),
                SpriteEffects.None,
                0
            );
        }
        public static void DrawTriangle(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float thickness = 1f)
        {
            DrawLine(spriteBatch, p1, p2, color, thickness);
            DrawLine(spriteBatch, p2, p3, color, thickness);
            DrawLine(spriteBatch, p3, p1, color, thickness);
        }
        public static void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness = 1)
        {
            // Top
            DrawLine(spriteBatch, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), color, thickness);

            // Bottom
            DrawLine(spriteBatch, new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness);

            // Left
            DrawLine(spriteBatch, new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom), color, thickness);

            // Right
            DrawLine(spriteBatch, new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom), color, thickness);
        }

        public static void DrawPolygon(SpriteBatch spriteBatch, Vector2[] points, Color color, float thickness = 1f)
        {
            if (points.Length < 2)
                return;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 start = points[i];
                Vector2 end = points[(i + 1) % points.Length]; // wraps around

                DrawLine(spriteBatch, start, end, color, thickness);
            }
        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float thickness = 1f)
        {
            Vector2[] points = new Vector2[sides];

            for (int i = 0; i < sides; i++)
            {
                float angle = MathHelper.TwoPi * i / sides;
                points[i] = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
            }

            DrawPolygon(spriteBatch, points, color, thickness);
        }



        //------------------Helpers-----------------//
        public static Rectangle CreateRectangleFromTwoPoints(Point a, Point b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            int width = Math.Abs(a.X - b.X);
            int height = Math.Abs(a.Y - b.Y);

            return new Rectangle(x, y, width, height);
        }
    }
}
