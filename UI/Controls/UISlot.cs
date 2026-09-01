using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class UISlot : Control
    {
        public Texture2D Texture { get; set; }
        public Color TintColor { get; set; } = Color.White;
        private Rectangle textureRect = new Rectangle();
        public Color NormalBorderColor
        {
            get => _normalBorderColor;
            set
            {
                _normalBorderColor = value;
                BorderColor = _normalBorderColor;
            }
        }
        private Color _normalBorderColor = Color.Black;
        public Color HighlightBorderColor { get; set; } = Color.Yellow;
        public Label Label { get; private set; }
        public string Text
        {
            get => Label.Text;
            set
            {
                Label.Text = value;
            }
        }
        public Color FontColor
        {
            get => Label.FontColor;
            set
            {
                Label.FontColor = value;
            }
        }
        public int Padding { get; set; } = 5;

        public Action<UISlot> OnClick;

        public UISlot() : base()
        {
            Label = new Label
            {
                Text = "0",
            };
        }
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

            float xLabel = Position.X + Padding;
            float yLabel = (Bounds.Bottom - Label.TextSize.Y) - Padding;
            Label.Position = new Vector2(xLabel, yLabel);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Texture != null)
            {
                spriteBatch.Draw(Texture, textureRect, TintColor);
            }

            Label.Draw(spriteBatch);
        }
        public override void MouseEnter(UIMouseEvent e)
        {
            BorderColor = HighlightBorderColor;
        }
        public override void MouseExit(UIMouseEvent e)
        {
            BorderColor = NormalBorderColor;
        }
        public override void MouseDown(UIMouseEvent e)
        {
            OnClick?.Invoke(this);
        }
    }
}
