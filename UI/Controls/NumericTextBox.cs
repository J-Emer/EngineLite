using System;
using Microsoft.Xna.Framework;

namespace EngineLite.UI.Controls
{
    public class NumericTextBox : TextBox
    {
        public int Value { get; private set; } = 0;
        public Action<int> OnValueChanged;


        public NumericTextBox() : base()
        {
            Text = "0";
            HasFocus = false;
            Size = new Vector2(150, 25);
        }
        protected override bool IsCharAllowed(char c)
        {
            return char.IsDigit(c);
        }
        protected override void HandleTextFinished()
        {
            Value = int.Parse(Text);
            OnValueChanged?.Invoke(Value);
        }
    }
}
