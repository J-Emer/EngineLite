using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Util
{
    public class MainCamera
    {
        public static MainCamera Instance { get; private set; }

        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;

        public float Zoom
        {
            get => _zoom;
            set => _zoom = Math.Clamp(value, MinZoom, MaxZoom);
        }
        private float _zoom = 1f;

        public float MinZoom { get; set; } = 0.25f;
        public float MaxZoom { get; set; } = 5f;

        private float _padding = 0f;
        private readonly Viewport _viewport;

        /// <summary>
        /// Automatically calculates an axis-aligned bounding box (AABB) 
        /// around the rotated and zoomed camera viewport.
        /// </summary>
        public Rectangle VisibleWorldBounds
        {
            get
            {
                Matrix inverseView = Matrix.Invert(GetViewMatrix());

                // 1. Define the 4 corners of the screen/viewport
                Vector2 topLeft = Vector2.Transform(new Vector2(0, 0), inverseView);
                Vector2 topRight = Vector2.Transform(new Vector2(_viewport.Width, 0), inverseView);
                Vector2 bottomLeft = Vector2.Transform(new Vector2(0, _viewport.Height), inverseView);
                Vector2 bottomRight = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseView);

                // 2. Find the minimum and maximum world coordinates extents
                float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
                float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
                float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
                float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

                // 3. Apply the manual padding offset configured by UpdateBoundsSize
                minX -= _padding;
                minY -= _padding;
                float width = (maxX - minX) + (_padding * 2);
                float height = (maxY - minY) + (_padding * 2);

                return new Rectangle((int)minX, (int)minY, (int)width, (int)height);
            }
        }

        public MainCamera(Viewport viewport)
        {
            _viewport = viewport;
            Instance = this;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(_viewport.Width / 2f, _viewport.Height / 2f, 0);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(GetViewMatrix()));
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, GetViewMatrix());
        }

        public bool IsOnScreen(Rectangle bounds)
        {
            return VisibleWorldBounds.Intersects(bounds);
        }

        public bool IsOnScreen(Vector2 position)
        {
            return VisibleWorldBounds.Contains(position);
        }

        public bool IsOnScreen(Point position)
        {
            return VisibleWorldBounds.Contains(position);
        }

        /// <summary>
        /// Adjusts the expansion padding of the VisibleWorldBounds rectangle.
        /// Use this to inflate the culling zone so objects do not pop out on screen edges.
        /// </summary>
        public void UpdateBoundsSize(float paddingValue)
        {
            _padding = paddingValue;
        }

        public override string ToString()
        {
            return $"[Camera]: Position: {Position} | Rotation: {Rotation} | Zoom: {Zoom} | MinZoom: {MinZoom} | MaxZoom: {MaxZoom} | Visible World Bounds: {VisibleWorldBounds}";
        }
    }
}
