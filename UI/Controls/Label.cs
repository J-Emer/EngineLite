using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class Label : Control
    {
        public SpriteFont Font { get; set; } = GameUIRenderer.Instance.DefaultFont;
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; } = "Lable";
        public Vector2 TextSize => Font.MeasureString(Text);

        public Label()
        {
            Name = "Label";
            BackgroundColor = Color.Transparent;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Font, Text, Position, FontColor);
        }
    }
}
