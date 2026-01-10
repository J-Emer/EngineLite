using System;
using System.Collections.Generic;

namespace EngineLite.Engine.IO
{
    public class SceneData
    {
        public HashSet<int> Entities;
        public Dictionary<Type, object> ComponentPools;

        public SceneData(HashSet<int> _entities, Dictionary<Type, object> _pools)
        {
            Entities = _entities;
            ComponentPools = _pools;
        }
    }
}