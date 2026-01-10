using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace EngineLite.Engine.ECS.Components
{
    public struct SpriteSheet
    {
        [JsonIgnore]
        public Texture2D Texture;
        public string TextureName
        {
            get
            {
               return _textureName; 
            }
            set
            {
                if(_textureName != value)
                {
                    _textureName = value;
                    Texture = AssetLoader.GetTexture(_textureName);
                }
            }
        }
        private string _textureName;
        public Color DrawColor{get;set;} = Color.White;
        public int DrawLayer{get;set;} = 0;
        public SpriteEffects Effects{get;set;} = SpriteEffects.None;
        public readonly Vector2 TextureSize => new Vector2(Texture.Width, Texture.Height);
        public Rectangle SourceRectangle{get;set;} = new Rectangle();

        
        public SpriteSheet()
        {
        }
    }
}