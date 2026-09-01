using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI.Controls
{
    public class Panel : ContainerControl
    {
        public Panel() : base()
        {
            Name = "Panel";
            Size = new Vector2(300, 300);
        }
        
    }
}
