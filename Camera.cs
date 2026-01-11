using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite
{
    public class Camera
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0f;
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = Math.Clamp(value, MinZoom, MaxZoom);
            }
        }
        private float _zoom = 1f;
        public float MinZoom{get;set;} = 0.25f;
        public float MaxZoom{get;set;} = 5f;




        private readonly Viewport _viewport;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1f) *
                Matrix.CreateTranslation(
                    _viewport.Width * 0.5f,
                    _viewport.Height * 0.5f,
                    0f
                );
        }

        // Convert screen → world
        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(GetViewMatrix()));
        }

        // Convert world → screen
        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, GetViewMatrix());
        }

    }
}
