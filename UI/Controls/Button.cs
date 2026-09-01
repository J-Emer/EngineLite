using System;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class Button : Control
    {
        public Color HighlightColor { get; set; } = Color.Blue;
        public Color NormalColor { get; set; } = Color.Orange;
        public SpriteFont Font { get; set; } = GameUIRenderer.Instance.DefaultFont;
        public Color FontColor { get; set; } = Color.Black;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                HandleDirty();
            }
        }
        private string _text = "Button";

        private Vector2 _textPos = Vector2.Zero;
        public Action<Button> OnClick;



        public Button() : base()
        {
            Name = "Button";
            BackgroundColor = NormalColor;
            Size = new Vector2(150, 30);
        }

        protected override void AfterDirty()
        {
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 center = Bounds.Center.ToVector2();

            float x = center.X - (textSize.X / 2);
            float y = center.Y - (textSize.Y / 2);

            _textPos = new Vector2(x, y);

            base.AfterDirty();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, Text, _textPos, FontColor);
        }

        public override void MouseEnter(UIMouseEvent e)
        {
            BackgroundColor = HighlightColor;
        }
        public override void MouseExit(UIMouseEvent e)
        {
            BackgroundColor = NormalColor;
        }
        public override void MouseDown(UIMouseEvent e)
        {
            OnClick?.Invoke(this);
        }
    }
}
