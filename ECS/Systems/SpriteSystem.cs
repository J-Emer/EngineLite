using System;
using EngineLite.Engine.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Systems
{
    public class SpriteSystem : DrawSystem
    {
        public override void Draw(SpriteBatch _spritebatch)
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<Transform, Sprite>())
            {
                EntityWorld.Instance.GetComponent(entityId, out Transform transform);
                EntityWorld.Instance.GetComponent(entityId, out Sprite sprite);

                //_spritebatch.Draw(sprite.Texture, transform.Position, sprite.DrawColor);


                Vector2 origin = sprite.TextureSize * 0.5f;

                _spritebatch.Draw(
                                    sprite.Texture, 
                                    transform.Position, 
                                    null,//source rectangel 
                                    sprite.DrawColor, 
                                    transform.Rotation,
                                    origin,//origin 
                                    transform.Scale,//scale
                                    sprite.Effects, 
                                    sprite.DrawLayer * 0.1f
                                );
            }
        }

        public void Other()
        {
            Console.WriteLine("---this is the SpriteSystem:Other()");
        }
    }
}