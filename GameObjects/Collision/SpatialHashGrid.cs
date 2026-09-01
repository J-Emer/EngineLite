using System;
using System.Collections.Generic;


namespace EngineLite.GameObjects.Collision
{
    /// <summary>
    /// Uniform spatial hash grid used to accelerate broad-phase collision queries.
    /// Each collider is hashed into every cell its AABB overlaps, so lookups only
    /// need to check colliders that are actually nearby instead of every collider
    /// in the world.
    /// </summary>
    public class SpatialHashGrid
    {
        private readonly float _cellSize;
        private readonly Dictionary<long, List<BoxCollider>> _cells = new();

        /// <param name="cellSize">
        /// Should roughly match the size of your average collider. Too small = many
        /// cells per object (overhead); too large = many objects per cell (defeats the purpose).
        /// </param>
        public SpatialHashGrid(float cellSize = 64f)
        {
            if (cellSize <= 0) throw new ArgumentException("cellSize must be > 0", nameof(cellSize));
            _cellSize = cellSize;
        }

        private long Key(int cx, int cy)
        {
            // Pack two 32-bit ints into one long so we get a fast, allocation-free dictionary key.
            return ((long)cx << 32) ^ (uint)cy;
        }

        private (int cx, int cy) CellCoords(float x, float y)
        {
            return ((int)Math.Floor(x / _cellSize), (int)Math.Floor(y / _cellSize));
        }

        public void Clear()
        {
            _cells.Clear();
        }

        public void Insert(BoxCollider collider)
        {
            AABB bounds = collider.Bounds;
            var (minCx, minCy) = CellCoords(bounds.MinX, bounds.MinY);
            var (maxCx, maxCy) = CellCoords(bounds.MaxX, bounds.MaxY);

            for (int cx = minCx; cx <= maxCx; cx++)
            {
                for (int cy = minCy; cy <= maxCy; cy++)
                {
                    long key = Key(cx, cy);
                    if (!_cells.TryGetValue(key, out var list))
                    {
                        list = new List<BoxCollider>();
                        _cells[key] = list;
                    }
                    list.Add(collider);
                }
            }
        }

        /// <summary>
        /// Returns every unique unordered pair of colliders that share at least one cell.
        /// Static-static pairs are skipped since two immovable objects never need resolving.
        /// </summary>
        public HashSet<(BoxCollider a, BoxCollider b)> GetPotentialPairs()
        {
            var pairs = new HashSet<(BoxCollider, BoxCollider)>();

            foreach (var list in _cells.Values)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = i + 1; j < list.Count; j++)
                    {
                        BoxCollider a = list[i];
                        BoxCollider b = list[j];

                        if (a.IsStatic && b.IsStatic) continue;

                        // Order consistently by ID so the same pair found via two
                        // different shared cells collapses to one entry in the set.
                        pairs.Add(a.ID <= b.ID ? (a, b) : (b, a));
                    }
                }
            }

            return pairs;
        }

        /// <summary>
        /// Returns every collider registered in cells overlapped by the given area.
        /// This is a broad-phase result — callers should still do a precise AABB
        /// check against the exact query area (see CollisionSystem.BoxOverlap).
        /// </summary>
        public List<BoxCollider> QueryArea(AABB area)
        {
            var result = new HashSet<BoxCollider>();
            var (minCx, minCy) = CellCoords(area.MinX, area.MinY);
            var (maxCx, maxCy) = CellCoords(area.MaxX, area.MaxY);

            for (int cx = minCx; cx <= maxCx; cx++)
            {
                for (int cy = minCy; cy <= maxCy; cy++)
                {
                    if (_cells.TryGetValue(Key(cx, cy), out var list))
                    {
                        foreach (var c in list) result.Add(c);
                    }
                }
            }

            return new List<BoxCollider>(result);
        }
    }
}
