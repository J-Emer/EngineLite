using EngineLite.Engine.ECS.Components;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Systems
{
    public class PlayerFacerSystem : UpdateSystem
    {
        public override void Update()
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<PhysicsBody, Sprite>())
            {
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physicsBody);
                EntityWorld.Instance.GetComponent(entityId, out Sprite sprite);

                float X = physicsBody.Body.LinearVelocity.X;

                bool isFacingRight = true;

                if(X < 0)
                {
                    //moving left
                    isFacingRight = false;
                }

                if(X != 0)
                {
                    if(!isFacingRight && sprite.Effects == SpriteEffects.None)
                    {
                        sprite.Effects = SpriteEffects.FlipHorizontally;
                    }

                    if(isFacingRight && sprite.Effects != SpriteEffects.None)
                    {
                        sprite.Effects = SpriteEffects.None;
                    }
                }

                EntityWorld.Instance.SetComponent(entityId, sprite);
            }
        }
    }
}