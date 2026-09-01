using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.UI.Controls;
using Microsoft.Xna.Framework;

namespace EngineLite.UI.Util

{

    public abstract class Layout
    {
        public abstract void HandleLayout(List<Control> controls, int padding, Rectangle bounds);
    }

    public class VerticalLayout : Layout
    {
        public override void HandleLayout(List<Control> controls, int padding, Rectangle bounds)
        {
            float xPos = bounds.X + padding;
            float ypos = bounds.Y + padding;

            foreach (var item in controls)
            {
                item.Position = new Vector2(xPos, ypos);
                ypos += item.Size.Y + padding;
            }
        }
    }

    public class RowLayout : Layout
    {
        public override void HandleLayout(List<Control> controls, int padding, Rectangle bounds)
        {
            float xPos = bounds.X + padding;
            float ypos = bounds.Y + padding;
            float width = bounds.Width - (padding * 2);

            foreach (var item in controls)
            {
                float height = item.Size.Y;

                item.Position = new Vector2(xPos, ypos);
                item.Size = new Vector2(width, height);
                ypos += item.Size.Y + padding;
            }
        }
    }

    public class ColumnLayout : Layout
    {
        public override void HandleLayout(List<Control> controls, int padding, Rectangle bounds)
        {
            float x = bounds.X + padding;
            float y = bounds.Y + padding;

            foreach (var item in controls)
            {
                item.Position = new Vector2(x, y);
                x += item.Size.X + padding;
            }
        }
    }

    public class CenterStretch : Layout
    {
        public override void HandleLayout(List<Control> controls, int padding, Rectangle bounds)
        {
            if (controls.Count == 0) { return; }

            Control control = controls[0];

            control.Position = new Vector2(bounds.X + padding, bounds.Y + padding);
            control.Size = new Vector2(bounds.Width - padding, bounds.Height - padding);
        }
    }

    public class GridLayout : Layout
    {
        public Vector2 CellSize { get; set; } = new Vector2(32, 32);
        public int Columns { get; set; } = 3;
        public int Rows { get; set; } = 3;

        public GridLayout(Vector2 cellSize, int columns, int rows)
        {
            CellSize = cellSize;
            Columns = columns;
            Rows = rows;
        }
        public override void HandleLayout(List<Control> controls, int padding, Rectangle bounds)
        {
            float startX = bounds.X + padding;
            float startY = bounds.Y + padding;

            for (int i = 0; i < controls.Count; i++)
            {
                int row = i / Columns;
                int column = i % Columns;

                // Stop if we've exceeded the grid size
                if (row >= Rows)
                    break;

                float x = startX + (column * (CellSize.X + padding));
                float y = startY + (row * (CellSize.Y + padding));

                controls[i].Position = new Vector2(x, y);
                controls[i].Size = CellSize;
            }
        }
    }
    
}
