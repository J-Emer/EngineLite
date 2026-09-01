using EngineLite.Core;
using EngineLite.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace EngineLite.GameObjects.Components
{
    public class Sprite : Component
    {
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        public string TextureName { get; set; }

        public Rectangle SourceRectangle { get; set; } = new Rectangle();
        public SpriteEffects Effects { get; set; } = SpriteEffects.None;
        public Color DrawColor { get; set; } = Color.White;
        public float DrawLayer { get; set; } = 0f;

        public Sprite(Texture2D texture, Rectangle sourceRectangle)
        {
            Texture = texture;
            TextureName = Texture.Name;
            SourceRectangle = sourceRectangle;
        }

        public Sprite()
        {
            
        }

        public override void Start()
        {
            if(Texture == null)
            {
                Texture = AssetLoader.GetTexture(TextureName);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(Texture == null) { return; }
            
            if(!MainCamera.Instance.IsOnScreen(Transform.Position)){return;}

            Transform transform = GameObject.GetComponent<Transform>();
            
            Vector2 origin = new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
            
            spriteBatch.Draw(
                Texture,
                transform.Position,
                SourceRectangle,
                DrawColor,
                transform.Rotation,
                origin,
                transform.Scale,
                Effects,
                DrawLayer
            );
        }
    }
}
