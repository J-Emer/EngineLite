using System;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public class Label : UIComponent
    {

        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Label";
        public Color FontColor{get;set;} = Color.Black;

        public Label() : base()
        {
            Scale = new Vector2(30, 30);
            BackgroundColor = Color.Transparent;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawString(Font, Text, Position, FontColor);
        }
    }    
}