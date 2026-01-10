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

        private bool _isGrounded = false;

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

                if (Input.GetKeyDown(Keys.Space))
                {
                    _isGrounded = false;

                    Vector2 start = ConvertUnits.ToDisplayUnits(physicsBody.Body.Position);
                    Vector2 end = start + new Vector2(0, 20);

                    PhysicsWorld.World.RayCast(
                        DoJump,
                        ConvertUnits.ToSimUnits(start),
                        ConvertUnits.ToSimUnits(end)
                    );

                    if (_isGrounded)
                    {
                        physicsBody.Body.ApplyLinearImpulse(
                            ConvertUnits.ToSimUnits(new Vector2(0, -25f))
                        );
                    }
                }

                EntityWorld.Instance.SetComponent(entityId, playerMover);
                EntityWorld.Instance.SetComponent(entityId, physicsBody);
            }
        }

        private float DoJump(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            if(fixture.Body.BodyType == BodyType.Static)
            {
                int entityId = (int)fixture.Body.Tag;

                if(EntityWorld.Instance.HasComponent<Tag>(entityId))
                {
                    EntityWorld.Instance.GetComponent(entityId, out Tag tag);

                    if(tag.TagName == "Ground")
                    {
                        _isGrounded = true;
                    }
                }
            }

            return 1f; // continue ray
        }

    }
}