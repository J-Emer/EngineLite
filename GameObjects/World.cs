using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.GameObjects
{
    public class World
    {
        // public static World Instance { get; private set; }
        private List<GameObject> gameObjects = new List<GameObject>();
        private int nextID = 0;

        private List<GameObject> _toadd = new List<GameObject>();
        private List<GameObject> _toremove = new List<GameObject>();

        public int GameObjectsCount => gameObjects.Count;
        public int ToAddCount => _toadd.Count;
        public int ToRemoveCount => _toremove.Count;


        public event Action OnGameobjectsChanged;


        public World()
        {
            Engine.Instance.SceneManager.BeforeSceneChange += Clear;
        }

        private int GetNextID()
        {
            nextID += 1;
            return nextID;
        }

        /// <summary>
        /// Adds a new Gameobject to the World
        /// </summary>
        /// <param name="name">The Name of the Gameobject - Name should be unique</param>
        /// <param name="tag">The Tag used to group/query this Gameobject (defaults to "Default")</param>
        /// <returns>The newly created GameObject. Note: New Gameobjects are placed in a queue and won't appear in World queries or receive Start() until the next Update() is called</returns>
        public GameObject CreateGameobject(string name, string tag = "Default")
        {
            GameObject go = new GameObject
            {
                Name = name,
                Tag = tag,
                ID = GetNextID()
            };
            
            _toadd.Add(go);
            return go;
        }
        public void RegisterGameObject(GameObject go)
        {
            _toadd.Add(go);
            nextID = go.ID;
        }

        /// <summary>
        /// Removes a Gameobject from the World
        /// </summary>
        /// <param name="gameObject">The Gameobject to be destroyed</param>
        public void DestroyGameObject(GameObject gameObject)
        {
            _toremove.Add(gameObject);
        }

        private void Swap()
        {
            if(_toadd.Count == 0 && _toremove.Count == 0) { return; }

            gameObjects.AddRange(_toadd);
            
            foreach (var item in _toadd)
            {
                item.Start();
            }

            gameObjects.RemoveAll(item => _toremove.Contains(item));

            foreach (var item in _toremove)
            {
                item.OnDestroy();
            }

            _toadd.Clear();
            _toremove.Clear();

            OnGameobjectsChanged?.Invoke();
        }

        /// <summary>
        /// Calls Update() on all the Gameobjects & Components
        /// </summary>
        public void Update()
        {
            Swap();

            foreach (var item in gameObjects)
            {
                item.Update();
            }
        }

        /// <summary>
        /// Calls Draw() on all of the Gameobjects & Components
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (var item in gameObjects)
            {
                item.Draw(spriteBatch);
            }
        }


        private void Clear()
        {
            _toadd.ForEach(x => x.OnDestroy());
            gameObjects.ForEach(x => x.OnDestroy());

            _toadd.Clear();
            _toremove.Clear();
            gameObjects.Clear();

            OnGameobjectsChanged?.Invoke();
        }

        /// <summary>
        /// Returns a Gameobject based on its ID
        /// </summary>
        /// <param name="id">the <int>ID of the gameobject to be returned</param>
        /// <returns></returns>
        public GameObject GetGameObject(int id)
        {
            return gameObjects.FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// Read-only view of all active GameObjects currently in the world.
        /// (Does not include objects still pending in _toadd until the next Swap())
        /// </summary>
        public List<GameObject> GetAllGameObjects()
        {
            return gameObjects;
        }

        public List<GameObject> GetAllToAdd() => _toadd;

        public IEnumerable<GameObject> FindGameObjectsWithTag(string tag)
        {
            return gameObjects.Where(go => go.Tag == tag);
        }

        public GameObject FindGameObjectWithTag(string tag)
        {
            return gameObjects.FirstOrDefault(go => go.Tag == tag);
        }

        public GameObject FindGameObjectByName(string name)
        {
            return gameObjects.FirstOrDefault(go => go.Name == name);
        }

        public IEnumerable<GameObject> FindGameObjectsByName(string name)
        {
            return gameObjects.Where(go => go.Name == name);
        }

        /// <summary>
        /// Finds all GameObjects that have a component of type T attached.
        /// </summary>
        public IEnumerable<GameObject> FindGameObjectsWithComponent<T>() where T : Component
        {
            return gameObjects.Where(go => go.HasComponent<T>());
        }

        /// <summary>
        /// Finds all components of type T across every GameObject in the world.
        /// Useful for systems that need to iterate all instances of a component (e.g. all Rigidbodies).
        /// </summary>
        public IEnumerable<T> FindComponentsOfType<T>() where T : Component
        {
            return gameObjects.SelectMany(go => go.Components.OfType<T>());
        }

    }
}
