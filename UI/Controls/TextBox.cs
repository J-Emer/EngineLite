using System;
using EngineLite.Core;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EngineLite.UI.Controls
{
    public class TextBox : Control
    {
        public SpriteFont Font { get; set; } = GameUIRenderer.Instance.DefaultFont;
        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? string.Empty;
                // _caretIndex = Math.Min(_caretIndex, _text.Length);
                AfterDirty();
            }
        }
        private string _text = "TextBox";
        public Color FontColor { get; set; } = Color.Black;
        private int Padding = 5;
        private Vector2 _textPos = Vector2.Zero;
        private int _caretIndex = 0;
        public Action<string> OnTextCompleted;


        private int _caretTime = 30;
        private int _timer = 0;
        private bool _showCaret = false;



        public TextBox() : base()
        {
            HasFocus = false;
            Size = new Vector2(150, 25);
        }
        protected override void FocusChanged()
        {
            _showCaret = HasFocus;

            if (!_showCaret)
            {
                _timer = 0;
            }
        }
        protected override void AfterDirty()
        {
            _textPos = Position + new Vector2(Padding, Padding);
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            //ScissorStack.Push(Bounds);

            spritebatch.DrawString(Font, Text, _textPos, FontColor);

            if (HasFocus)
            {
                _timer += 1;

                if (_timer >= _caretTime)
                {
                    _showCaret = !_showCaret;
                    _timer = 0;
                }

                DrawCaret(spritebatch);
            }

            //ScissorStack.Pop();
        }
        private void DrawCaret(SpriteBatch spriteBatch)
        {
            if (!_showCaret) { return; }

            int safeCaret = Math.Clamp(_caretIndex, 0, Text.Length);

            string leftText = safeCaret > 0 ? Text.Substring(0, safeCaret) : string.Empty;

            float caretX = _textPos.X + Font.MeasureString(leftText).X;

            Rectangle caretRect = new Rectangle(
                (int)caretX,
                (int)_textPos.Y,
                1,
                Font.LineSpacing
            );

            spriteBatch.Draw(GameUIRenderer.Instance.Pixel, caretRect, FontColor);
        }
        public override void KeyDown(Keys key)
        {
            if (!IsActive) { return; }

            bool shiftDown = Input.ShiftDown();

            if (HandleSpecialKeys(key))
            {
                AfterDirty();
                return;
            }

            char? c = KeyToChar(key, shiftDown);

            if (c.HasValue)
            {
                if (IsCharAllowed(c.Value))
                {
                    Text = Text.Insert(_caretIndex, c.Value.ToString());
                    _caretIndex++;
                }
                AfterDirty();
            }
        }

        //----------------------Key Helpers---------------------------//

        private bool HandleSpecialKeys(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    if (_caretIndex > 0 && Text.Length > 0)
                    {
                        Text = Text.Remove(_caretIndex - 1, 1);
                        _caretIndex--;
                    }
                    return true;

                case Keys.Enter:
                    HasFocus = false;
                    HandleTextFinished();
                    return true;

                case Keys.Space:
                    Text = Text.Insert(_caretIndex, " ");
                    _caretIndex++;
                    return true;

                case Keys.Tab:
                    const string tabSpaces = "    ";
                    Text = Text.Insert(_caretIndex, tabSpaces);
                    _caretIndex += tabSpaces.Length;
                    return true;

                case Keys.Left:
                    _caretIndex = Math.Max(0, _caretIndex - 1);
                    return true;

                case Keys.Right:
                    _caretIndex = Math.Min(Text.Length, _caretIndex + 1);
                    return true;

            }

            return false;
        }
        private char? KeyToChar(Keys key, bool shift)
        {
            // Letters
            if (key >= Keys.A && key <= Keys.Z)
            {
                char c = (char)('a' + (key - Keys.A));
                return shift ? char.ToUpper(c) : c;
            }

            // Numbers (top row)
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return shift ? ShiftNumber(key) : (char)('0' + (key - Keys.D0));
            }

            // Numpad
            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                return (char)('0' + (key - Keys.NumPad0));
            }

            // Symbols
            return key switch
            {
                Keys.OemPeriod => shift ? '>' : '.',
                Keys.Decimal => '.',
                Keys.OemComma => shift ? '<' : ',',
                Keys.OemMinus => shift ? '_' : '-',
                Keys.OemPlus => shift ? '+' : '=',
                Keys.OemQuestion => shift ? '?' : '/',
                Keys.OemSemicolon => shift ? ':' : ';',
                Keys.OemQuotes => shift ? '"' : '\'',
                Keys.OemOpenBrackets => shift ? '{' : '[',
                Keys.OemCloseBrackets => shift ? '}' : ']',
                Keys.OemPipe => shift ? '|' : '\\',
                Keys.OemTilde => shift ? '~' : '`',
                _ => null
            };
        }
        private char ShiftNumber(Keys key)
        {
            return key switch
            {
                Keys.D1 => '!',
                Keys.D2 => '@',
                Keys.D3 => '#',
                Keys.D4 => '$',
                Keys.D5 => '%',
                Keys.D6 => '^',
                Keys.D7 => '&',
                Keys.D8 => '*',
                Keys.D9 => '(',
                Keys.D0 => ')',
                _ => '\0'
            };
        }
        protected virtual bool IsCharAllowed(char c)
        {
            return true;
        }
        protected virtual void HandleTextFinished()
        {
            HasFocus = false;
            OnTextCompleted?.Invoke(Text);
        }






    }
}
