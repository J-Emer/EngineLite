using System;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public class Slot : UITexture
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "0";
        public Color FontColor{get;set;} = Color.Black;
        public int Padding = 5;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 _fontSize = Font.MeasureString(Text);

            int x = SourceRect.X + Padding;
            int y = SourceRect.Bottom - ((int)_fontSize.Y + Padding);

            spriteBatch.DrawString(Font, Text, new Vector2(x, y), FontColor);
        }
    }
}