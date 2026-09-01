using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite;
using EngineLite.GameObjects.Components;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace EngineLite.GameObjects
{
    public abstract class Component
    {
        public int ID { get; set; } = -1;
        
        [JsonIgnore]
        public GameObject GameObject
        {
            get
            {
                if(_gameObject == null)
                {
                    _gameObject = Engine.Instance.World.GetGameObject(ID);
                }

                return _gameObject;
            }
        }
        private GameObject _gameObject = null;
        
        [JsonIgnore]
        public Transform Transform
        {
            get
            {
                if(_transform == null)
                {
                    _transform = GameObject.GetComponent<Transform>();
                }

                return _transform;
            }
        }
        private Transform _transform;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnEnabledChanged();
            }
        }
        private bool _isEnabled = true;

        
        public virtual void Start() { }
        public virtual void Update() {}
        public virtual void Draw(SpriteBatch spriteBatch){}
        public virtual void OnDestroy() { }
        protected virtual void OnEnabledChanged(){}


        public override string ToString()
        {
            return $"Name: {GetType().Name} |  ID: {ID}";
        }
    
    
    }
}
