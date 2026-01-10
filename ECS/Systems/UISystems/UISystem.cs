using System.Collections.Generic;
using EngineLite.Engine.ECS.Components.UI;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Systems.UISystems
{
    public class UISystem
    {
        public static UISystem Instance;

        public List<Panel> Children = new List<Panel>();

        public UISystem()
        {
            Instance = this;
        }

        public void Update()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(spriteBatch);
            }
        }        
    }
}