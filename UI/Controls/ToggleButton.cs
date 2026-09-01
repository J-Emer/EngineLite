using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class ToggleButton : Control
    {
        public Color ThumbColor { get; set; } = Color.Blue;
        public Color ThumbColorActive { get; set; } = Color.Blue;
        public Color ThumbColorInActive { get; set; } = Color.Black;


        public int ThumbSize { get; set; } = 20;
        private Rectangle thumbRect = new Rectangle();
        private bool _value = true;
        public bool Value { get => _value; }
        private int _padding = 5;


        public Action<bool> OnValueChanged;







        public ToggleButton() : base()
        {
            Size = new Vector2(70, 30);
        }
        protected override void AfterDirty()
        {
            int x = 0;

            if(_value)
            {
                x = (int)(Position.X + _padding);
                ThumbColor = ThumbColorActive;
            }
            else
            {
                x = Bounds.Right - (ThumbSize + _padding);
                ThumbColor = ThumbColorInActive;
            }


            thumbRect = new Rectangle(
                                        x, 
                                        (int)(Position.Y + _padding), 
                                        ThumbSize,
                                        ThumbSize
                                      );
        }

        public override void MouseDown(UIMouseEvent e)
        {
            if(thumbRect.Contains(e.Position))
            {
                _value = !_value;
                OnValueChanged?.Invoke(_value);
                AfterDirty();
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(GameUIRenderer.Instance.Pixel, thumbRect, ThumbColor);
        }
    }
}
