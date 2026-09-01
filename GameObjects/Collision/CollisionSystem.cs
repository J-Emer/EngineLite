using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EngineLite.GameObjects.Components;

namespace EngineLite.GameObjects.Collision
{
    /// <summary>
    /// Owns and steps the AABB collision simulation: registers colliders, runs broad-phase
    /// via a SpatialHashGrid, does narrow-phase AABB overlap tests, resolves overlaps by
    /// pushing objects apart along the minimum penetration axis, fires
    /// OnCollisionEnter/Stay/Exit on the involved BoxColliders, and exposes Raycast and
    /// BoxOverlap queries.
    ///
    /// Usage:
    ///   var collisionSystem = new CollisionSystem(cellSize: 64f);
    ///   collisionSystem.Register(someBoxCollider);
    ///   // each frame, after moving objects, before drawing:
    ///   collisionSystem.Update();
    /// </summary>
    public sealed class CollisionSystem
    {
        // private static readonly CollisionSystem _instance = new CollisionSystem();
        // public static CollisionSystem Instance => _instance;

        private  SpatialHashGrid _grid;
        private  List<BoxCollider> _colliders = new();

        // Pairs that were overlapping last frame, used to distinguish Enter vs Stay vs Exit.
        private HashSet<(BoxCollider a, BoxCollider b)> _previousPairs = new();

        // --- Debug draw ---
        /// <summary>Toggle to draw an outline rectangle around every registered collider.</summary>
        public bool DebugDrawEnabled { get; set; } = true;
        public Color DebugStaticColor { get; set; } = Color.Yellow;
        public Color DebugDynamicColor { get; set; } = Color.Lime;
        public Color DebugTriggerColor { get; set; } = Color.Cyan;
        public Color DebugCollidingColor { get; set; } = Color.Red;
        public int DebugLineThickness { get; set; } = 2;

        private Texture2D _debugPixel;

        public CollisionSystem(int cellSize = 64)
        {
            _grid = new SpatialHashGrid(cellSize);
        }

        // public void Initialize(float cellSize = 64f)
        // {
        //     _grid = new SpatialHashGrid(cellSize);
        // }

        public void Register(BoxCollider collider)
        {
            if (!_colliders.Contains(collider))
                _colliders.Add(collider);
        }

        public void Unregister(BoxCollider collider)
        {
            _colliders.Remove(collider);
        } 
        public void Clear()
        {
            foreach (var collider in _colliders)
            {
                collider.ActiveCollisions.Clear();
            }
        
            _colliders.Clear();
            _grid.Clear();
            _previousPairs.Clear();
        }
        /// <summary>
        /// Runs one full collision step: rebuild grid -> broad phase -> narrow phase ->
        /// resolve -> fire events. Call once per frame, after your GameObjects have moved.
        /// </summary>
        public void Update()
        {
            _grid.Clear();
            foreach (var c in _colliders)
            {
                if (c.IsEnabled)
                    _grid.Insert(c);
            }

            var candidatePairs = _grid.GetPotentialPairs();
            var currentPairs = new HashSet<(BoxCollider, BoxCollider)>();

            foreach (var (a, b) in candidatePairs)
            {
                if (!a.IsEnabled || !b.IsEnabled) continue;

                AABB boundsA = a.Bounds;
                AABB boundsB = b.Bounds;

                if (!boundsA.Intersects(boundsB)) continue;

                currentPairs.Add((a, b));

                CollisionResult result = ComputeCollision(boundsA, boundsB);
                bool isTriggerPair = a.IsTrigger || b.IsTrigger;

                if (!isTriggerPair)
                {
                    ResolvePositions(a, b, result);
                }

                FireOverlapEvents(a, b, result, isTriggerPair);
            }

            FireExitEvents(currentPairs);

            _previousPairs = currentPairs;
        }

        private struct CollisionResult
        {
            /// <summary>Unit vector that would push "a" away from "b".</summary>
            public Vector2 NormalAtoB;
            public float Penetration;
            public AABB Overlap;
        }

        private static CollisionResult ComputeCollision(AABB a, AABB b)
        {
            float overlapX = Math.Min(a.MaxX, b.MaxX) - Math.Max(a.MinX, b.MinX);
            float overlapY = Math.Min(a.MaxY, b.MaxY) - Math.Max(a.MinY, b.MinY);

            var overlapBounds = new AABB(
                Math.Max(a.MinX, b.MinX),
                Math.Max(a.MinY, b.MinY),
                Math.Min(a.MaxX, b.MaxX),
                Math.Min(a.MaxY, b.MaxY)
            );

            Vector2 normal;
            float penetration;

            // Push out along whichever axis has the smaller overlap - the minimum
            // translation vector (MTV). This is the standard, cheap AABB resolution
            // approach; it can be wrong for fast-moving objects (tunneling / wrong-axis
            // pick), which is what swept/continuous collision detection solves if you
            // need it later.
            if (overlapX < overlapY)
            {
                penetration = overlapX;
                normal = a.Center.X < b.Center.X ? new Vector2(-1f, 0f) : new Vector2(1f, 0f);
            }
            else
            {
                penetration = overlapY;
                normal = a.Center.Y < b.Center.Y ? new Vector2(0f, -1f) : new Vector2(0f, 1f);
            }

            return new CollisionResult
            {
                NormalAtoB = normal,
                Penetration = penetration,
                Overlap = overlapBounds
            };
        }

        private static void ResolvePositions(BoxCollider a, BoxCollider b, CollisionResult result)
        {
            if (a.IsStatic && b.IsStatic) return;

            Vector2 push = result.NormalAtoB * result.Penetration;

            if (a.IsStatic)
            {
                b.Transform.Position -= push;
            }
            else if (b.IsStatic)
            {
                a.Transform.Position += push;
            }
            else
            {
                // Both dynamic: split the correction evenly.
                a.Transform.Position += push * 0.5f;
                b.Transform.Position -= push * 0.5f;
            }
        }

        private void FireOverlapEvents(BoxCollider a, BoxCollider b, CollisionResult result, bool isTrigger)
        {
            bool wasOverlapping = _previousPairs.Contains((a, b));

            var infoForA = new CollisionInfo
            {
                Other = b,
                Normal = result.NormalAtoB,
                PenetrationDepth = result.Penetration,
                OverlapBounds = result.Overlap,
                IsTrigger = isTrigger
            };

            var infoForB = new CollisionInfo
            {
                Other = a,
                Normal = -result.NormalAtoB,
                PenetrationDepth = result.Penetration,
                OverlapBounds = result.Overlap,
                IsTrigger = isTrigger
            };

            if (wasOverlapping)
            {
                a.HandleCollisionStay(infoForA);
                b.HandleCollisionStay(infoForB);
            }
            else
            {
                a.ActiveCollisions.Add(b);
                b.ActiveCollisions.Add(a);
                a.HandleCollisionEnter(infoForA);
                b.HandleCollisionEnter(infoForB);
            }
        }

        private void FireExitEvents(HashSet<(BoxCollider, BoxCollider)> currentPairs)
        {
            foreach (var (a, b) in _previousPairs)
            {
                if (currentPairs.Contains((a, b))) continue;

                bool wasTrigger = a.IsTrigger || b.IsTrigger;

                a.ActiveCollisions.Remove(b);
                b.ActiveCollisions.Remove(a);

                a.HandleCollisionExit(new CollisionInfo { Other = b, IsTrigger = wasTrigger });
                b.HandleCollisionExit(new CollisionInfo { Other = a, IsTrigger = wasTrigger });
            }
        }

        // ------------------------------------------------------------------
        // Raycast
        // ------------------------------------------------------------------
 
        /// <summary>
        /// Casts a ray from <paramref name="origin"/> in <paramref name="direction"/> and
        /// returns the closest BoxCollider it hits within maxDistance, if any.
        /// </summary>
        public RaycastHit Raycast(
            Vector2 origin,
            Vector2 direction,
            float maxDistance = float.MaxValue,
            Func<BoxCollider, bool> filter = null)
        {
            if (direction == Vector2.Zero) return RaycastHit.None;
            direction.Normalize();
 
            float queryDistance = maxDistance == float.MaxValue ? 100000f : maxDistance;
            Vector2 end = origin + direction * queryDistance;
            AABB queryArea = AABB.FromSegment(origin, end);
 
            var candidates = _grid.QueryArea(queryArea);
 
            RaycastHit closest = RaycastHit.None;
            float closestDistance = maxDistance;
 
            foreach (var collider in candidates)
            {
                if (!collider.IsEnabled) continue;
                if (filter != null && !filter(collider)) continue;
 
                if (TryIntersectRayAABB(origin, direction, collider.Bounds, closestDistance, out float t, out Vector2 normal))
                {
                    closestDistance = t;
                    closest = new RaycastHit
                    {
                        Hit = true,
                        Collider = collider,
                        Point = origin + direction * t,
                        Normal = normal,
                        Distance = t
                    };
                }
            }
 
            return closest;
        }
 
        /// <summary>
        /// Casts a ray from <paramref name="start"/> to <paramref name="end"/> and returns
        /// the closest BoxCollider hit, if any. Convenience overload for when you have two
        /// points rather than an origin/direction/distance.
        /// </summary>
        public RaycastHit Raycast(Vector2 start, Vector2 end)
        {
            Vector2 delta = end - start;
            float distance = delta.Length();
            if (distance < 0.0001f) return RaycastHit.None;
 
            return Raycast(start, delta, distance);
        }
 
        /// <summary>
        /// Casts a ray from <paramref name="start"/> to <paramref name="end"/>, ignoring any
        /// collider belonging to <paramref name="ignoredEntity"/> - e.g. so a ray fired from
        /// the player doesn't immediately hit the player's own collider.
        /// </summary>
        public RaycastHit Raycast(Vector2 start, Vector2 end, GameObject ignoredEntity)
        {
            Vector2 delta = end - start;
            float distance = delta.Length();
            if (distance < 0.0001f) return RaycastHit.None;
 
            return Raycast(start, delta, distance, collider => collider.GameObject != ignoredEntity);
        }
 
        /// <summary>
        /// Casts a ray and returns every BoxCollider it hits within maxDistance, sorted
        /// nearest-first - e.g. for a piercing bullet that should hit every enemy in a line
        /// instead of stopping at the first one.
        /// </summary>
        public List<RaycastHit> RaycastAll(
            Vector2 origin,
            Vector2 direction,
            float maxDistance = float.MaxValue,
            Func<BoxCollider, bool> filter = null)
        {
            var hits = new List<RaycastHit>();
 
            if (direction == Vector2.Zero) return hits;
            direction.Normalize();
 
            float queryDistance = maxDistance == float.MaxValue ? 100000f : maxDistance;
            Vector2 end = origin + direction * queryDistance;
            AABB queryArea = AABB.FromSegment(origin, end);
 
            var candidates = _grid.QueryArea(queryArea);
 
            foreach (var collider in candidates)
            {
                if (!collider.IsEnabled) continue;
                if (filter != null && !filter(collider)) continue;
 
                if (TryIntersectRayAABB(origin, direction, collider.Bounds, maxDistance, out float t, out Vector2 normal))
                {
                    hits.Add(new RaycastHit
                    {
                        Hit = true,
                        Collider = collider,
                        Point = origin + direction * t,
                        Normal = normal,
                        Distance = t
                    });
                }
            }
 
            hits.Sort((h1, h2) => h1.Distance.CompareTo(h2.Distance));
            return hits;
        }
 
        /// <summary>RaycastAll convenience overload for two points instead of origin/direction/distance.</summary>
        public List<RaycastHit> RaycastAll(Vector2 start, Vector2 end)
        {
            Vector2 delta = end - start;
            float distance = delta.Length();
            if (distance < 0.0001f) return new List<RaycastHit>();
 
            return RaycastAll(start, delta, distance);
        }
 
        /// <summary>RaycastAll convenience overload that also ignores colliders belonging to ignoredEntity.</summary>
        public List<RaycastHit> RaycastAll(Vector2 start, Vector2 end, GameObject ignoredEntity)
        {
            Vector2 delta = end - start;
            float distance = delta.Length();
            if (distance < 0.0001f) return new List<RaycastHit>();
 
            return RaycastAll(start, delta, distance, collider => collider.GameObject != ignoredEntity);
        }



        /// <summary>Slab-method ray/AABB intersection. Returns the nearest entry point within [0, maxDistance].</summary>
        private static bool TryIntersectRayAABB(Vector2 origin, Vector2 dir, AABB box, float maxDistance, out float tHit, out Vector2 normal)
        {
            const float epsilon = 1e-8f;

            float tMin = 0f;
            float tMax = maxDistance;
            Vector2 hitNormal = Vector2.Zero;

            // X slab
            if (Math.Abs(dir.X) < epsilon)
            {
                if (origin.X < box.MinX || origin.X > box.MaxX)
                {
                    tHit = 0f;
                    normal = Vector2.Zero;
                    return false;
                }
            }
            else
            {
                float invDx = 1f / dir.X;
                float t1 = (box.MinX - origin.X) * invDx;
                float t2 = (box.MaxX - origin.X) * invDx;
                Vector2 n1 = new Vector2(-1f, 0f);

                if (t1 > t2)
                {
                    (t1, t2) = (t2, t1);
                    n1 = new Vector2(1f, 0f);
                }

                if (t1 > tMin) { tMin = t1; hitNormal = n1; }
                if (t2 < tMax) tMax = t2;

                if (tMin > tMax)
                {
                    tHit = 0f;
                    normal = Vector2.Zero;
                    return false;
                }
            }

            // Y slab
            if (Math.Abs(dir.Y) < epsilon)
            {
                if (origin.Y < box.MinY || origin.Y > box.MaxY)
                {
                    tHit = 0f;
                    normal = Vector2.Zero;
                    return false;
                }
            }
            else
            {
                float invDy = 1f / dir.Y;
                float t1 = (box.MinY - origin.Y) * invDy;
                float t2 = (box.MaxY - origin.Y) * invDy;
                Vector2 n1 = new Vector2(0f, -1f);

                if (t1 > t2)
                {
                    (t1, t2) = (t2, t1);
                    n1 = new Vector2(0f, 1f);
                }

                if (t1 > tMin) { tMin = t1; hitNormal = n1; }
                if (t2 < tMax) tMax = t2;

                if (tMin > tMax)
                {
                    tHit = 0f;
                    normal = Vector2.Zero;
                    return false;
                }
            }

            if (tMin < 0f || tMin > maxDistance)
            {
                tHit = 0f;
                normal = Vector2.Zero;
                return false;
            }

            tHit = tMin;
            normal = hitNormal;
            return true;
        }

        // ------------------------------------------------------------------
        // Box overlap query
        // ------------------------------------------------------------------

        /// <summary>Returns every enabled collider that overlaps the given area.</summary>
        public List<BoxCollider> BoxOverlap(AABB area, Func<BoxCollider, bool> filter = null)
        {
            var results = new List<BoxCollider>();

            foreach (var collider in _grid.QueryArea(area))
            {
                if (!collider.IsEnabled) continue;
                if (filter != null && !filter(collider)) continue;
                if (collider.Bounds.Intersects(area)) results.Add(collider);
            }

            return results;
        }

        /// <summary>Convenience overload: overlap test for a box centered at <paramref name="center"/>.</summary>
        public List<BoxCollider> BoxOverlap(Vector2 center, Vector2 size, Func<BoxCollider, bool> filter = null)
        {
            return BoxOverlap(AABB.FromCenterSize(center, size), filter);
        }

        // ------------------------------------------------------------------
        // Debug draw
        // ------------------------------------------------------------------

        /// <summary>
        /// Draws an outline rectangle around every registered, enabled collider. No-ops if
        /// <see cref="DebugDrawEnabled"/> is false, so it's safe to call unconditionally every
        /// frame and just flip the flag to toggle it. Call between spriteBatch.Begin()/End(),
        /// same as your normal sprite drawing.
        ///
        /// Colors: yellow = static, green = dynamic, cyan = trigger, red = currently overlapping
        /// something (overrides the others so active collisions stand out).
        /// </summary>
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            if (!DebugDrawEnabled) return;

            EnsureDebugPixel(spriteBatch.GraphicsDevice);

            foreach (var collider in _colliders)
            {
                if (!collider.IsEnabled) continue;

                Color color;
                if (collider.ActiveCollisions.Count > 0)
                    color = DebugCollidingColor;
                else if (collider.IsTrigger)
                    color = DebugTriggerColor;
                else if (collider.IsStatic)
                    color = DebugStaticColor;
                else
                    color = DebugDynamicColor;

                DrawRectangleOutline(spriteBatch, collider.Bounds, color, DebugLineThickness);
            }
        }

        private void EnsureDebugPixel(GraphicsDevice graphicsDevice)
        {
            if (_debugPixel != null) return;

            _debugPixel = new Texture2D(graphicsDevice, 1, 1);
            _debugPixel.SetData(new[] { Color.White });
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, AABB bounds, Color color, int thickness)
        {
            Vector2 topLeft = new Vector2(bounds.MinX, bounds.MinY);
            Vector2 topRight = new Vector2(bounds.MaxX, bounds.MinY);
            Vector2 bottomLeft = new Vector2(bounds.MinX, bounds.MaxY);
            Vector2 bottomRight = new Vector2(bounds.MaxX, bounds.MaxY);

            DrawLine(spriteBatch, topLeft, topRight, color, thickness);       // top
            DrawLine(spriteBatch, bottomLeft, bottomRight, color, thickness); // bottom
            DrawLine(spriteBatch, topLeft, bottomLeft, color, thickness);     // left
            DrawLine(spriteBatch, topRight, bottomRight, color, thickness);   // right
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness)
        {
            Vector2 delta = end - start;
            float length = delta.Length();
            if (length < 0.0001f) return;

            float angle = (float)Math.Atan2(delta.Y, delta.X);

            spriteBatch.Draw(
                _debugPixel,
                start,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0f
            );
        }

        /// <summary>
        /// Releases the 1x1 texture used for debug drawing. Call this from your
        /// UnloadContent if you created a CollisionSystem with debug draw ever enabled.
        /// </summary>
        public void UnloadDebugResources()
        {
            _debugPixel?.Dispose();
            _debugPixel = null;
        }
    }
}

