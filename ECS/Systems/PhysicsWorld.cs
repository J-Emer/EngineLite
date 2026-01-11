using System;
using System.Collections.Generic;
using EngineLite.Engine.Core;
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
        public static World World { get; private set; }

        private static readonly Queue<CollisionEvent> _collisionQueue = new();

        // ---------------------------------------------
        // Initialization
        // ---------------------------------------------
        public static void Init()
        {
            World = new World(new Vector2(0, 9.8f)); // TODO: pull from settings

            World.ContactManager.BeginContact += OnBeginContact;
            World.ContactManager.EndContact += OnEndContact;
        }

        // ---------------------------------------------
        // Update
        // ---------------------------------------------
        public static void Update()
        {
            World.Step(Time.DeltaTime);

            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<Transform, PhysicsBody, BoxCollider>())
            {
                EntityWorld.Instance.GetComponent(entityId, out Transform transform);
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physicsBody);
                EntityWorld.Instance.GetComponent(entityId, out BoxCollider collider);

                if (!physicsBody.HasInitialized)
                {
                    InitializeBody(entityId, ref transform, ref physicsBody, ref collider);
                }

                // Sync transform from physics
                transform.Position =
                    ConvertUnits.ToDisplayUnits(physicsBody.Body.Position);

                physicsBody.Rotation = physicsBody.Body.Rotation;

                // Push updated structs back into ECS
                EntityWorld.Instance.SetComponent(entityId, transform);
                EntityWorld.Instance.SetComponent(entityId, physicsBody);
                EntityWorld.Instance.SetComponent(entityId, collider);
            }

            ProcessCollisions();
        }

        // ---------------------------------------------
        // Cleanup
        // ---------------------------------------------
        public static void Clear()
        {
            _collisionQueue.Clear();
            World.Clear();
        }

        // ---------------------------------------------
        // Body / Fixture creation
        // ---------------------------------------------
        private static void InitializeBody(int entityId, ref Transform transform, ref PhysicsBody physicsBody,ref BoxCollider collider)
        {
            physicsBody.Body = World.CreateBody(
                ConvertUnits.ToSimUnits(transform.Position),
                ConvertUnits.ToSimUnits(physicsBody.Rotation),
                physicsBody.BodyType);

            // ECS <-> Physics link
            physicsBody.Body.Tag = entityId;

            float width  = ConvertUnits.ToSimUnits(collider.Size.X);
            float height = ConvertUnits.ToSimUnits(collider.Size.Y);
            Vector2 offset = ConvertUnits.ToSimUnits(collider.Offset);

            collider.Fixture = physicsBody.Body.CreateRectangle(
                width,
                height,
                collider.Density,
                offset);

            collider.HasInitialized = true;
            physicsBody.HasInitialized = true;
        }

        // ---------------------------------------------
        // Aether collision callbacks
        // ---------------------------------------------
        private static bool OnBeginContact(Contact contact)
        {
            EnqueueCollision(contact, CollisionType.Begin);
            return true; // allow physics resolution
        }

        private static void OnEndContact(Contact contact)
        {
            EnqueueCollision(contact, CollisionType.End);
        }

        private static void EnqueueCollision(Contact contact, CollisionType type)
        {
            var bodyA = contact.FixtureA.Body;
            var bodyB = contact.FixtureB.Body;

            if (bodyA.Tag is not int entityA ||
                bodyB.Tag is not int entityB)
                return;

            _collisionQueue.Enqueue(new CollisionEvent
            {
                EntityA = entityA,
                EntityB = entityB,
                Contact = contact,
                Type = type
            });
        }

        // ---------------------------------------------
        // ECS collision dispatch
        // ---------------------------------------------
        private static void ProcessCollisions()
        {
            while (_collisionQueue.Count > 0)
            {
                var evt = _collisionQueue.Dequeue();

                Dispatch(evt.EntityA, evt);
                Dispatch(evt.EntityB, evt);
            }
        }

        private static void Dispatch(int entityId, CollisionEvent evt)
        {
            if (!EntityWorld.Instance.HasComponent<OnCollisionComponent>(entityId))
                return;

            EntityWorld.Instance.GetComponent(entityId, out OnCollisionComponent component);

            component.Callback?.Invoke(evt);
        }
    }

    public enum CollisionType
    {
        Begin,
        End
    }

    public struct CollisionEvent
    {
        public int EntityA;
        public int EntityB;
        public Contact Contact;
        public CollisionType Type;
    }

}
