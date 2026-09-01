using System.Collections.Generic;
using System.Linq;
using EngineLite.Core;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public abstract class ContainerControl : Control
    {
        private List<Control> controls = new List<Control>();
        public IReadOnlyList<Control> Controls => controls;
        public int ControlCount => controls.Count;
        public int Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                AfterDirty();
            }
        }
        private int _padding = 5;
        public Layout Layout
        {
            get => _layout;
            set
            {
                _layout = value;
                AfterDirty();
            }
        }
        private Layout _layout = new RowLayout();


        public Control Find(string name)
        {
            return controls.FirstOrDefault(x => x.Name == name);
        }
        public T Find<T>(string name) where T : Control
        {
            return (T)controls.FirstOrDefault(x => x.Name == name);
        }
        protected override void ActiveChanged()
        {
            foreach (var item in controls)
            {
                item.IsActive = IsActive;
            }
        }
        public void Add(Control control)
        {
            controls.Add(control);
            AfterDirty();
        }
        public void Remove(Control control)
        {
            controls.Remove(control);
            AfterDirty();
        }
        public override Control MouseHitTest()
        {
            if (!IsActive) { return null; }

            foreach (var item in controls)
            {
                var hit = item.MouseHitTest();

                if (hit != null)
                {
                    return hit;
                }
            }

            if (Bounds.Contains(Input.MousePosition))
            {
                return this;
            }

            return null;
        }
        protected override void AfterDirty()
        {
            Layout.HandleLayout(controls, Padding, Bounds);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) { return; }

            base.Draw(spriteBatch);

            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Draw(spriteBatch);
            }
        }
    }
}
