using EngineLite.Engine.Core;
using EngineLite.Engine.ECS.Components;
using EngineLite.Engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;

namespace EngineLite.Engine.ECS.Systems
{
    public class PlayerMoverSystem : UpdateSystem
    {
        public override void Update()
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<PhysicsBody, PlayerMover>())
            {
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physicsBody);
                EntityWorld.Instance.GetComponent(entityId, out PlayerMover playerMover);

                Vector2 input = new Vector2(Input.XAxis, 0);

                Vector2 accel = input * playerMover.Speed;
                Vector2 force = physicsBody.Body.Mass * accel;

                if(Input.XAxis == 0)
                {
                    physicsBody.Body.LinearVelocity = new Vector2(0, physicsBody.Body.LinearVelocity.Y);
                }

                physicsBody.Body.ApplyForce(force);


                EntityWorld.Instance.SetComponent(entityId, playerMover);
                EntityWorld.Instance.SetComponent(entityId, physicsBody);
            }
        }



    }
}