using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using nkast.Aether.Physics2D.Dynamics;

namespace EngineLite.Engine.ECS.Components
{
    public struct PhysicsBody
    {
        [JsonIgnore]
        public Body Body;
        public bool HasInitialized{get;set;} = false;
        public float Rotation{get;set;} = 0f;
        public BodyType BodyType{get;set;} = BodyType.Static;
    
    
        public PhysicsBody()
        {
        }
    
    }
}