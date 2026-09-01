using Microsoft.Xna.Framework;

namespace EngineLite.GameObjects.Collision
{
    /// <summary>
    /// Axis-Aligned Bounding Box using float precision (XNA's Rectangle is int-only,
    /// which loses precision for sub-pixel movement).
    /// </summary>
    public struct AABB
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;
        public Vector2 Center => new Vector2((MinX + MaxX) * 0.5f, (MinY + MaxY) * 0.5f);
        public Vector2 Size => new Vector2(Width, Height);
        public Vector2 Min => new Vector2(MinX, MinY);
        public Vector2 Max => new Vector2(MaxX, MaxY);

        public AABB(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public static AABB FromCenterSize(Vector2 center, Vector2 size)
        {
            Vector2 half = size * 0.5f;
            return new AABB(
                center.X - half.X,
                center.Y - half.Y,
                center.X + half.X,
                center.Y + half.Y
            );
        }

        /// <summary>
        /// Builds an AABB that tightly encloses a line segment. Used to narrow down
        /// spatial-grid queries before doing a precise raycast test.
        /// </summary>
        public static AABB FromSegment(Vector2 a, Vector2 b)
        {
            return new AABB(
                MathHelper.Min(a.X, b.X),
                MathHelper.Min(a.Y, b.Y),
                MathHelper.Max(a.X, b.X),
                MathHelper.Max(a.Y, b.Y)
            );
        }

        public bool Intersects(AABB other)
        {
            return MinX < other.MaxX && MaxX > other.MinX &&
                   MinY < other.MaxY && MaxY > other.MinY;
        }

        public bool Contains(Vector2 point)
        {
            return point.X >= MinX && point.X <= MaxX &&
                   point.Y >= MinY && point.Y <= MaxY;
        }

        public AABB Expanded(float amount)
        {
            return new AABB(MinX - amount, MinY - amount, MaxX + amount, MaxY + amount);
        }

        public override string ToString() => $"[{MinX:0.##},{MinY:0.##}] -> [{MaxX:0.##},{MaxY:0.##}]";
    }
}
