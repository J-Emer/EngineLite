using System;
using System.Text.Json.Serialization;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public class UITexture : UIComponent
    {
        private bool PMouse = false;
        private bool CMouse = false;
        private bool _showBorder = false;
        public Color BorderColor{get;set;} = Color.Black;
        public int BorderThickness = 2;
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
        private string _textureName = null;
        
        
        
        public Action OnClick;

        public UITexture() : base()
        {
            Scale = new Vector2(30, 30);
            BackgroundColor = Color.White;
        }

        public override void Update()
        {
            PMouse = CMouse;
            CMouse = SourceRect.Contains(Input.MousePos);

            //enter
            if(!PMouse && CMouse)
            {
                _showBorder = true;
            }

            //clicked
            if(CMouse && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                Console.WriteLine("UITexture: clicked");
                OnClick?.Invoke();
            }

            //exit
            if(PMouse && !CMouse)
            {
                _showBorder = false;
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(Texture != null)
            {
                Vector2 _textureSize = new Vector2(Texture.Width, Texture.Height);
                Vector2 _halfTextureSize = _textureSize * 0.5f;

                spriteBatch.Draw(Texture, Center - _halfTextureSize, BackgroundColor);                
            }

            if(!_showBorder){return;}

            spriteBatch.Draw(AssetLoader.GetPixel(), new Rectangle(SourceRect.Left, SourceRect.Top, SourceRect.Width, BorderThickness), BorderColor);//top
            spriteBatch.Draw(AssetLoader.GetPixel(), new Rectangle(SourceRect.Right - BorderThickness, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//right
            spriteBatch.Draw(AssetLoader.GetPixel(), new Rectangle(SourceRect.Left, SourceRect.Bottom - BorderThickness, SourceRect.Width, BorderThickness), BorderColor);//bottom
            spriteBatch.Draw(AssetLoader.GetPixel(), new Rectangle(SourceRect.Left, SourceRect.Top, BorderThickness, SourceRect.Height), BorderColor);//left            
        }
    }    
}