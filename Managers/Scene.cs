using System;
using EngineLite.Engine.ECS;

namespace EngineLite.Engine.Managers
{
    public abstract class Scene
    {
        public string Title { get; private set; }
    
        protected Scene(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            SceneManager.Instance?.Add(this); // safe add if manager exists
        }
    
        /// <summary>
        /// Load resources, entities, etc.
        /// </summary>
        public abstract void Load();
    
        /// <summary>
        /// Unload resources, entities, etc.
        /// </summary>
        public virtual void Unload()
        {
            EntityWorld.Instance.Clear();
        }
    }
}
