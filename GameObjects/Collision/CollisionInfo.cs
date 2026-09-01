using Microsoft.Xna.Framework;
using EngineLite.GameObjects.Components;

namespace EngineLite.GameObjects.Collision
{
    /// <summary>
    /// Data passed to OnCollisionEnter / OnCollisionStay / OnCollisionExit, describing
    /// a collision from the perspective of "this" collider hitting <see cref="Other"/>.
    /// </summary>
    public class CollisionInfo
    {
        /// <summary>The other collider involved in this collision.</summary>
        public BoxCollider Other { get; internal set; } = null;
        public GameObject GameObject => Other.GameObject;

        /// <summary>
        /// Direction that would move "this" collider out of <see cref="Other"/>
        /// along the axis of least penetration (unit vector, e.g. (0,-1) = push up).
        /// </summary>
        public Vector2 Normal { get; internal set; }

        /// <summary>How far "this" collider is overlapping <see cref="Other"/> along the resolution axis.</summary>
        public float PenetrationDepth { get; internal set; }

        /// <summary>The overlapping rectangle between the two colliders (world space).</summary>
        public AABB OverlapBounds { get; internal set; }

        /// <summary>True if either collider involved is a trigger (no physical resolution occurred).</summary>
        public bool IsTrigger { get; internal set; }
    }

    /// <summary>
    /// Result of a <see cref="CollisionSystem.Raycast"/> call.
    /// </summary>
    public struct RaycastHit
    {
        public bool Hit;
        public BoxCollider Collider;
        public GameObject GameObject => Collider.GameObject;
        public Vector2 Point;
        public Vector2 Normal;
        public float Distance;

        public static readonly RaycastHit None = new RaycastHit { Hit = false };
    }
}
