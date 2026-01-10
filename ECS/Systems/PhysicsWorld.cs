using System;
using System.Reflection;
using EngineLite.Engine.ECS.Components;
using EngineLite.Engine.Utility;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

namespace EngineLite.Engine.ECS.Systems
{
    public static class PhysicsWorld
    {
        public static World World{get; private set;}

        public static void Init()
        {
            World = new World(new Vector2(0, 9.8f));   
        }
        public static void Update()
        {
            World.Step(1f/60f);//todo: change this should NOT hard code this

            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<Transform, PhysicsBody, BoxCollider>())
            {
                EntityWorld.Instance.GetComponent(entityId, out Transform transform);
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physicsBody);
                EntityWorld.Instance.GetComponent(entityId, out BoxCollider collider);


                if(!physicsBody.HasInitialized)
                {
                    InitializeBody(entityId, ref transform, ref physicsBody, ref collider);
                }


                transform.Position = ConvertUnits.ToDisplayUnits(physicsBody.Body.Position);
                physicsBody.Body.Rotation = 0;


                EntityWorld.Instance.SetComponent(entityId, transform);
                EntityWorld.Instance.SetComponent(entityId, physicsBody);
                EntityWorld.Instance.SetComponent(entityId, collider);

            }
        }
        public static void Clear()
        {
            World.Clear();
        }

        private static void InitializeBody(int entityID, ref Transform transform, ref PhysicsBody body, ref BoxCollider collider)
        {
            body.Body = World.CreateBody(ConvertUnits.ToSimUnits(transform.Position), ConvertUnits.ToSimUnits(transform.Rotation), body.BodyType);
            body.Body.Tag = entityID;

            float width = ConvertUnits.ToSimUnits(collider.Size.X);
            float height = ConvertUnits.ToSimUnits(collider.Size.Y);
            Vector2 offset = ConvertUnits.ToSimUnits(collider.Offset);
            
            collider.Fixture = body.Body.CreateRectangle(width, height, collider.Density, offset);

            collider.Fixture.OnCollision += Collision;

            body.HasInitialized = true;
            collider.HasInitialized = true;
        }

        private static bool Collision(Fixture sender, Fixture other, Contact contact)
        {
            HandleCollision(
                (int)sender.Body.Tag,
                (int)other.Body.Tag,
                contact
            );
            return true;
        }

        private static void HandleCollision(int senderEntity, int otherEntity, Contact contact)
        {
            //this is where you will handle the entity specific collisions
        }

    }
}