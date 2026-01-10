using System;
using EngineLite.Engine.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Systems
{
    public class SpriteSheetSystem : DrawSystem
    {
        public override void Draw(SpriteBatch _spritebatch)
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<Transform, SpriteSheet>())
            {
                EntityWorld.Instance.GetComponent(entityId, out Transform transform);
                EntityWorld.Instance.GetComponent(entityId, out SpriteSheet spriteSheet);

                //_spritebatch.Draw(sprite.Texture, transform.Position, sprite.DrawColor);

                float x = spriteSheet.SourceRectangle.Width * 0.5f;
                float y = spriteSheet.SourceRectangle.Height * 0.5f;

                Vector2 origin = new Vector2(x, y);

                _spritebatch.Draw(
                                    spriteSheet.Texture, 
                                    transform.Position, 
                                    spriteSheet.SourceRectangle,//source rectangel 
                                    spriteSheet.DrawColor, 
                                    transform.Rotation,
                                    origin,//origin 
                                    transform.Scale,//scale
                                    spriteSheet.Effects, 
                                    spriteSheet.DrawLayer * 0.1f
                                );
            }
        }
    }
}