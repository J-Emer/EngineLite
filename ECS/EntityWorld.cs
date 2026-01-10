using System;
using System.Collections.Generic;
using EngineLite.Engine.EngineDebug;
using EngineLite.Engine.IO;

namespace EngineLite.Engine.ECS
{
    public class EntityWorld
    {
        public static EntityWorld Instance { get; private set; } 
        private int _nextEntityId = 1;
        private HashSet<int> _entities = new HashSet<int>();
        private Dictionary<Type, object> _componentPools = new Dictionary<Type, object>();

        public EntityWorld()
        {
            Stats.Instance.Add("Entity Count", EntityCount_String);
            Stats.Instance.Add("Component Pools", ComponentPoolsCount_String);

            Instance = this;  
        } 

        // Entity management
        public Entity CreateEntity()
        {
            int id = _nextEntityId++;
            _entities.Add(id);
            return new Entity(id);
        }

        public void DestroyEntity(Entity entity)
        {
            _entities.Remove(entity.Id);

            foreach (var pool in _componentPools.Values)
            {
                var dictType = pool.GetType();
                var removeMethod = dictType.GetMethod("Remove");
                removeMethod.Invoke(pool, new object[] { entity.Id });
            }
        }

        // Component management
        public void AddComponent<T>(Entity entity, T component) where T : struct
        {
            if (!_componentPools.ContainsKey(typeof(T)))
                _componentPools[typeof(T)] = new Dictionary<int, T>();

            var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
            pool[entity.Id] = component;
        }

        public void RemoveComponent<T>(Entity entity) where T : struct
        {
            if (_componentPools.ContainsKey(typeof(T)))
            {
                var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
                pool.Remove(entity.Id);
            }
        }

        // public bool GetComponent<T>(Entity entity, out T component) where T : struct
        // {
        //     component = default;
        //     if (!_componentPools.ContainsKey(typeof(T))) return false;
        //     var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
        //     return pool.TryGetValue(entity.Id, out component);
        // }

        public bool GetComponent<T>(int id, out T component) where T : struct
        {
            component = default;
            if (!_componentPools.ContainsKey(typeof(T))) return false;
            var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
            return pool.TryGetValue(id, out component);
        }

        // public void SetComponent<T>(Entity entity, T component) where T : struct
        // {
        //     if (!_componentPools.ContainsKey(typeof(T))) return;
        //     var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
        //     if (pool.ContainsKey(entity.Id))
        //         pool[entity.Id] = component;
        // }

        public void SetComponent<T>(int id, T component) where T : struct
        {
            if (!_componentPools.ContainsKey(typeof(T))) return;
            var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
            if (pool.ContainsKey(id))
                pool[id] = component;
        }

        public bool HasComponent<T>(int entityId) where T : struct
        {
            if (!_componentPools.TryGetValue(typeof(T), out var poolObj))
                return false;

            var pool = (Dictionary<int, T>)poolObj;
            return pool.ContainsKey(entityId);
        }

        public IEnumerable<int> GetEntitiesWithComponent<T>() where T : struct
        {
            if (!_componentPools.ContainsKey(typeof(T))) yield break;
            var pool = (Dictionary<int, T>)_componentPools[typeof(T)];
            foreach (var kvp in pool)
                yield return kvp.Key;
        }

        public IEnumerable<int> GetEntitiesWithComponents<T1, T2>() where T1 : struct where T2 : struct
        {
            if (!_componentPools.TryGetValue(typeof(T1), out var pool1Obj)) yield break;
            if (!_componentPools.TryGetValue(typeof(T2), out var pool2Obj)) yield break;

            var pool1 = (Dictionary<int, T1>)pool1Obj;
            var pool2 = (Dictionary<int, T2>)pool2Obj;

            // Iterate the smaller pool for efficiency
            if (pool1.Count <= pool2.Count)
            {
                foreach (var id in pool1.Keys)
                    if (pool2.ContainsKey(id))
                        yield return id;
            }
            else
            {
                foreach (var id in pool2.Keys)
                    if (pool1.ContainsKey(id))
                        yield return id;
            }
        }

        public IEnumerable<int> GetEntitiesWithComponents<T1, T2, T3>() where T1: struct where T2 : struct where T3 : struct
        {
            var set1 = GetEntitiesWithComponents<T1, T2>();
            foreach (var entityId in set1)
            {
                if (HasComponent<T3>(entityId))
                    yield return entityId;
            }
        }

        public void Clear()
        {
            _entities.Clear();
            _componentPools.Clear();
            _nextEntityId = 1;
        }

        //---Loads in our Entities & Component Pools from a file
        public void LoadFromFile(SceneData data)
        {
            Clear();

            // Restore entities
            foreach (var id in data.Entities)
            {
                _entities.Add(id);
                _nextEntityId = Math.Max(_nextEntityId, id + 1);
            }

            // Restore component pools
            foreach (var kvp in data.ComponentPools)
            {
                var componentType = kvp.Key;
                var poolObject = kvp.Value;

                // We trust SceneData to already contain Dictionary<int, T>
                _componentPools[componentType] = poolObject;
            }
        }




        //helper function for inspecting Entities & Components
        public HashSet<int> GetAllEntities()
        {
            return _entities;
        }

        public Dictionary<Type, object>GetAllComponents()
        {
            return _componentPools;
        } 

        public IEnumerable<(Type type, object component)> GetAllComponents(Entity entity)
        {
            foreach (var kvp in _componentPools)
            {
                var type = kvp.Key;
                var pool = kvp.Value;

                var containsMethod = pool.GetType().GetMethod("ContainsKey");
                if (!(bool)containsMethod.Invoke(pool, new object[] { entity.Id }))
                    continue;

                var indexer = pool.GetType().GetProperty("Item");
                var component = indexer.GetValue(pool, new object[] { entity.Id });

                yield return (type, component);
            }
        }

        public string EntityCount_String() 
        {
            return _entities.Count.ToString();
        }
        public string ComponentPoolsCount_String()
        {
             return _componentPools.Count.ToString();
        }

    }
}
