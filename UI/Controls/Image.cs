using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class Image : Control
    {
        public Texture2D Texture { get; set; } 
        public Color TintColor { get; set; } = Color.White;
        private Rectangle textureRect = new Rectangle();




        protected override void AfterDirty()
        {
            float x = Position.X + BorderThickness;
            float y = Position.Y + BorderThickness;
            float w = Size.X - (BorderThickness * 2);
            float h = Size.Y - (BorderThickness * 2);

            textureRect = new Rectangle(
                                            (int)x,
                                            (int)y,
                                            (int)w,
                                            (int)h
                                        );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Texture != null)
            {
                spriteBatch.Draw(Texture, textureRect, TintColor);
            }

        }

    }
}
