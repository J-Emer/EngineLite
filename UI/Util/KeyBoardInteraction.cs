using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.Core;
using Microsoft.Xna.Framework.Input;

namespace EngineLite.UI.Util

{
    public class KeyBoardInteraction
    {
        private MouseInteractions mouseInteractions;

        public KeyBoardInteraction(MouseInteractions _mouseInteractions)
        {
            mouseInteractions = _mouseInteractions;
        }

        public void Update()
        {
            if (mouseInteractions.FocusedControl == null)
            {
                return;
            }

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (Input.GetKeyDown(key))
                {
                    mouseInteractions.FocusedControl.KeyDown(key);
                }
                if (Input.GetKeyUp(key))
                {
                    mouseInteractions.FocusedControl.KeyUp(key);
                }
                if (Input.GetKey(key))
                {
                    mouseInteractions.FocusedControl.KeyHeld(key);
                }
            }
        }
    }
}
