using System;
using System.Collections.Generic;
using EngineLite.Core;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EngineLite.UI.Controls
{
    public class Control
    {
        public object UserData { get; set; } = null;
        public bool HasFocus
        {
            get => _hasFocus;
            set
            {
                _hasFocus = value;
                FocusChanged();
            }
        }
        private bool _hasFocus = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                ActiveChanged();
            }
        }
        private bool _isActive = true;
        public string Name { get; set; } = "Control";
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                HandleDirty();
            }
        }
        private Vector2 size = Vector2.One;
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size= value;
                HandleDirty();
            }
        }
        public Rectangle Bounds { get; private set; } = new Rectangle();
        public Color BackgroundColor { get; set; } = Color.DarkGray;

        public Color BorderColor { get; set; } = Color.Black;
        public int BorderThickness { get; set; } = 1;

        private Rectangle borderTop;
        private Rectangle borderBottom;
        private Rectangle borderLeft;
        private Rectangle borderRight;
        



        public Control()
        {

        }

        protected void HandleDirty()
        {
            Bounds = new Rectangle
                                  (
                                      (int)position.X,
                                      (int)position.Y,
                                      (int)size.X,
                                      (int)size.Y
                                  );

            borderTop = new Rectangle(
            Bounds.Left,
            Bounds.Top,
            Bounds.Width,
            BorderThickness);

            borderBottom = new Rectangle(
                Bounds.Left,
                Bounds.Bottom - BorderThickness,
                Bounds.Width,
                BorderThickness);

            borderLeft = new Rectangle(
                Bounds.Left,
                Bounds.Top,
                BorderThickness,
                Bounds.Height);

            borderRight = new Rectangle(
                Bounds.Right - BorderThickness,
                Bounds.Top,
                BorderThickness,
                Bounds.Height);

            AfterDirty();
        }
        public virtual Control MouseHitTest()
        {
            if (!IsActive) { return null; }

            if(Bounds.Contains(Input.MousePosition))
            {
                return this;
            }

            return null;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) { return; }

            spriteBatch.Draw(GameUIRenderer.Instance.Pixel, Bounds, BackgroundColor);

            if (BorderThickness > 0)
            {
                spriteBatch.Draw(GameUIRenderer.Instance.Pixel, borderTop, BorderColor);
                spriteBatch.Draw(GameUIRenderer.Instance.Pixel, borderBottom, BorderColor);
                spriteBatch.Draw(GameUIRenderer.Instance.Pixel, borderLeft, BorderColor);
                spriteBatch.Draw(GameUIRenderer.Instance.Pixel, borderRight, BorderColor);
            }
        }







        protected virtual void ActiveChanged() { }
        protected virtual void FocusChanged() { }
        protected virtual void AfterDirty() { }
        public virtual void MouseUp(UIMouseEvent e) { }
        public virtual void MouseDown(UIMouseEvent e) { }
        public virtual void MouseMove(UIMouseEvent e) { }
        public virtual void MouseEnter(UIMouseEvent e) { }
        public virtual void MouseExit(UIMouseEvent e) { }
        public virtual void MouseHover(UIMouseEvent e) { }
        public virtual void Scroll(UIMouseEvent e) { }
        public virtual void KeyDown(Keys key) { }
        public virtual void KeyUp(Keys key) { }
        public virtual void KeyHeld(Keys key) { }





    }
}
