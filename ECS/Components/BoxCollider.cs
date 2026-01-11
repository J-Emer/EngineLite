using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace EngineLite.Engine.ECS.Components
{
    public struct BoxCollider
    {
        [JsonIgnore]
        public Fixture Fixture{get;set;}

        public bool HasInitialized{get;set;} = false;
        public Vector2 Size{get;set;} = Vector2.One;
        public float Density{get;set;} = 1f;
        public Vector2 Offset{get;set;} = Vector2.Zero;



        public BoxCollider(Vector2 size)
        {
            Size = size;
        }
    }
}