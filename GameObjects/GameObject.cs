using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.GameObjects
{
    public class GameObject
    {
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if(_isEnabled != value)
                {
                    _isEnabled = value;

                    foreach (var item in Components)
                    {
                        item.IsEnabled = _isEnabled;
                    }
                }
            }
        }
        private bool _isEnabled = true;
        public string Name { get; set; } = "Gameobject";
        public string Tag { get; set; } = "Default";
        public int ID { get; set; } = -1;

        public List<Component> Components = new();


        public void AddComponent(Component component) 
        {
            Components.Add(component);
            component.ID = this.ID;
        }
        public void AddComponents(List<Component> comps)
        {
            foreach (var item in comps)
            {
                item.ID = this.ID;
                Components.Add(item);
            }
        }
        public void RemoveComponent(Component component)
        {
            Components.Remove(component);
        }
        public List<Component> GetComponents() => Components;
        public T GetComponent<T>() where T : Component
        {
            return Components.OfType<T>().FirstOrDefault();
        }
        public bool HasComponent<T>() where T : Component
        {
            return GetComponent<T>() != null;
        }
        public void Start()
        {
            foreach (var item in Components)
            {
                item.Start();
            }
        }
        public void Update()
        {
            if (!_isEnabled) { return; }

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].IsEnabled)
                {
                    Components[i].Update();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isEnabled) { return; }

            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].IsEnabled)
                {
                    Components[i].Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Call this when you want to Destroy a Gameobject
        /// </summary>
        public void Destroy()
        {
            Engine.Instance.World.DestroyGameObject(this);
        }

        /// <summary>
        /// Called by World when a Gameobject is being destroyed
        /// </summary>
        public void OnDestroy()
        {
            foreach (var item in Components)
            {
                item.OnDestroy();
            }
            Components.Clear();
        }
        public override string ToString()
        {
            return $"Name: {Name} | Tag: {Tag} | ID: {ID} | ComponentsCount: {Components.Count}";
        }
    }
}
