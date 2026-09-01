using System.Collections.Generic;
using Microsoft.Xna.Framework;
using EngineLite.GameObjects.Components;

namespace EngineLite.GameObjects.Collision
{
    /// <summary>
    /// Axis-aligned box collider component. Position is derived from the owning
    /// GameObject's Transform + an optional local Offset. Register instances with a
    /// <see cref="CollisionSystem"/> for them to actually be checked.
    /// </summary>
    public class BoxCollider : Component
    {
        /// <summary>
        /// Base (unscaled) size, analogous to a Sprite's SourceRectangle dimensions.
        /// Actual world-space size is this multiplied by Transform.Scale - see <see cref="ScaledSize"/>.
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>Local offset from Transform.Position (e.g. to center a collider on a sprite's feet). Scales with Transform.Scale, same as the offset would visually scale with the sprite.</summary>
        public Vector2 Offset { get; set; } = Vector2.Zero;

        /// <summary>Triggers detect and report overlaps but are never physically pushed apart.</summary>
        public bool IsTrigger { get; set; } = false;

        /// <summary>Static colliders (walls, floors, level geometry) never move during resolution.</summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>Size actually used for collision, after applying Transform.Scale.</summary>
        public Vector2 ScaledSize => Size * Transform.Scale;

        public Vector2 Position => Transform.Position + Offset * Transform.Scale;

        public AABB Bounds => AABB.FromCenterSize(Position, ScaledSize);

        // Tracked by CollisionSystem to know which pairs are currently overlapping,
        // so it can tell Enter apart from Stay apart from Exit.
        internal readonly HashSet<BoxCollider> ActiveCollisions = new();



        public Action<CollisionInfo> OnCollisionEnter;
        public Action<CollisionInfo> OnCollisionStay;
        public Action<CollisionInfo> OnCollisionExit;





        public BoxCollider(Vector2 size)
        {
            Size = size;
        }

        public override void Start()
        {
            Engine.Instance.CollisionSystem.Register(this);            
        }


        public BoxCollider(Vector2 size, Vector2 offset) : this(size)
        {
            Offset = offset;
        }

        /// <summary>Called the first frame two colliders start overlapping.</summary>
        public void HandleCollisionEnter(CollisionInfo info)
        {
            OnCollisionEnter?.Invoke(info);
        }

        /// <summary>Called every subsequent frame the two colliders remain overlapping.</summary>
        public void HandleCollisionStay(CollisionInfo info)
        {
            OnCollisionStay?.Invoke(info);
        }

        /// <summary>Called the frame two colliders stop overlapping.</summary>
        public void HandleCollisionExit(CollisionInfo info)
        {
            OnCollisionExit?.Invoke(info);
        }
        public override void OnDestroy()
        {
            Engine.Instance.CollisionSystem.Unregister(this);
        }








    }
}