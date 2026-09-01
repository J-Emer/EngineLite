using System;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class DropDown : Control
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
        private string _text = "DropDown";

        private Vector2 _textPos = Vector2.Zero;



        public Panel Panel { get; private set; }
        public Layout Layout => Panel.Layout;
        public int Padding => Panel.Padding;
        private bool _showPanel = false;


        public Action<Button> OnItemSelected;
        public Button SelectedItem { get; private set; }


        public DropDown() : base()
        {
            Panel = new Panel
            {
                Size = new Vector2(150, 300),
                Padding = 5
            };

            Name = "DropDown";
            BackgroundColor = NormalColor;
            Size = new Vector2(150, 30);
        }

        public void Add(string text)
        {
            if(Panel.ControlCount == 0)
            {
                Text = text;
            }
            Button button = new Button
            {
                Text = text
            };
            button.OnClick += ItemClicked;

            Panel.Add(button);
            AfterDirty();

        }
        public void Add(string text, object userdata)
        {
            if (Panel.ControlCount == 0)
            {
                Text = text;
            }

            Button button = new Button
            {
                Text = text,
                UserData = userdata
            };
            button.OnClick += ItemClicked;

            Panel.Add(button);
            AfterDirty();
        }
        public void Remove(Button button)
        {
            Panel.Remove(button);
            AfterDirty();
        }
        private void ItemClicked(Button button)
        {
            SelectedItem = button;
            Text = SelectedItem.Text;
            TogglePanel();
            OnItemSelected?.Invoke(SelectedItem);
        }
        protected override void AfterDirty()
        {
            Vector2 textSize = Font.MeasureString(Text);
            Vector2 center = Bounds.Center.ToVector2();

            float x = center.X - (textSize.X / 2);
            float y = center.Y - (textSize.Y / 2);

            _textPos = new Vector2(x, y);

            float xPos = Position.X;
            float yPos = Position.Y + Size.Y;


            int height = 0;
            foreach (var item in Panel.Controls)
            {
                height += (int)(Padding + item.Size.Y);
            }
            height += Padding;


            Panel.Position = new Vector2(xPos, yPos);
            Panel.Size = new Vector2(Size.X, height);

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
            TogglePanel();
        }
        private void TogglePanel()
        {
            _showPanel = !_showPanel;

            if (_showPanel)
            {
                GameUIRenderer.Instance.AddOverlay(Panel);
            }
            else
            {
                GameUIRenderer.Instance.RemoveOverlay(Panel);
            }
        }
    }
}
