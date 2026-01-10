using System.Collections.Generic;
using System.Linq;
using EngineLite.Engine.ECS.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS
{
    public class SystemsManager
    {
        public static SystemsManager Instance{get; private set;}
        private List<InitializeSystem> _initializeSystems = new List<InitializeSystem>();
        private List<UpdateSystem> _updatableSystems = new List<UpdateSystem>();
        private List<DrawSystem> _drawableSystems = new List<DrawSystem>();


        public SystemsManager()
        {
            Instance = this;
        }

        public void AddInitialize(InitializeSystem _system) => _initializeSystems.Add(_system);
        public void AddUpdatable(UpdateSystem _system) => _updatableSystems.Add(_system);
        public void AddDrawablde(DrawSystem _system) => _drawableSystems.Add(_system);


        public void RemoveInitialize(InitializeSystem _system) => _initializeSystems.Remove(_system);
        public void RemoveUpdatable(UpdateSystem _system) => _updatableSystems.Remove(_system);
        public void RemoveDrawablde(DrawSystem _system) => _drawableSystems.Remove(_system);


        public T GetInitializeSystem<T>() where T : InitializeSystem => (T)_initializeSystems.FirstOrDefault(x => x.GetType() == typeof(T));
        public T GetUpdateSystem<T>() where T : UpdateSystem => (T)_updatableSystems.FirstOrDefault(x => x.GetType() == typeof(T));
        public T GetDrawSystem<T>() where T : DrawSystem => (T)_drawableSystems.FirstOrDefault(x => x.GetType() == typeof(T));

        public void Initialize()
        {
            _initializeSystems.ForEach(x => x.Initialize());
        }
        public void Update()
        {
            _updatableSystems.ForEach(x => x.Update());
        }

        public void Draw(SpriteBatch _spritebatch)
        {
            _drawableSystems.ForEach(x => x.Draw(_spritebatch));
        }
    }
}