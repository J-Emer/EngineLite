using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Systems
{
    public abstract class System{}

    public abstract class InitializeSystem : System
    {
        public InitializeSystem()
        {
            SystemsManager.Instance.AddInitialize(this);
        }

        public abstract void Initialize();
    }
    public abstract class UpdateSystem : System
    {
        public UpdateSystem()
        {
            SystemsManager.Instance.AddUpdatable(this);
        }
        public abstract void Update();
    }
    public abstract class DrawSystem : System
    {
        public DrawSystem()
        {
            SystemsManager.Instance.AddDrawablde(this);
        }
        public abstract void Draw(SpriteBatch _spritebatch);
    }
}