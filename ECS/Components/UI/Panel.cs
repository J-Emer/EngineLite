using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public class Panel : UIComponent
    {
        public List<UIComponent> Children = new List<UIComponent>();
        public int Padding = 5;     
        public Layout Layout = new RowLayout();


        public override void Update()
        {
            base.Update();

            Layout.HandleLayout(SourceRect, Children, Padding);

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update();
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(spriteBatch);
            }
        }

    }

    public abstract class Layout
    {
        public abstract void HandleLayout(Rectangle parentRectangle, List<UIComponent> children, int Padding);
    }

    public class RowLayout : Layout
    {
        public override void HandleLayout(Rectangle parentRectangle, List<UIComponent> children, int Padding)
        {
            float childWidth = parentRectangle.Width - (Padding * 2);
            int xPos = parentRectangle.X + Padding;
            int yPos = parentRectangle.Y + Padding;

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Position = new Vector2(xPos, yPos);
                children[i].Scale = new Vector2(childWidth, children[i].Scale.Y);

                yPos += (int)children[i].Scale.Y + Padding;
            }
        }
    }

    public class GridLayout : Layout
    {
        public int Rows{get;set;} = 3;
        public int Columns{get;set;} = 3;

        public GridLayout(int rows, int cols)
        {
            Rows = rows;
            Columns = cols;
        }

        public override void HandleLayout(Rectangle sourceRect, List<UIComponent> controls, int padding)
        {
            int paddingTotal = (Columns + 1) * padding;
            int size = (sourceRect.Width - paddingTotal) / Columns;

            int xPos = sourceRect.X + padding;
            int yPos = sourceRect.Y + padding;

            int index = 0;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if(index >= controls.Count){return;}

                    UIComponent item = controls[index];

                    item.Position = new Vector2(xPos, yPos);
                    item.Scale = new Vector2(size, size);

                    xPos += size + padding;

                    index += 1;
                }
                
                xPos = sourceRect.X + padding;
                yPos += size + padding;
            }
        }
    }

    public class ColumnLayout : Layout
    {
        public override void HandleLayout(Rectangle parentRectangle, List<UIComponent> children, int Padding)
        {
            float childHeight = parentRectangle.Height - (Padding * 2);
            int xPos = parentRectangle.X + Padding;
            int yPos = parentRectangle.Y + Padding;

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Position = new Vector2(xPos, yPos);
                children[i].Scale = new Vector2(children[i].Scale.X, childHeight);

                xPos += (int)children[i].Scale.Y + Padding;
            }            
        }
    }

}