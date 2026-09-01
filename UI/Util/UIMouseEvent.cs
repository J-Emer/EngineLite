using Microsoft.Xna.Framework;

namespace EngineLite.UI.Util

{
    public class UIMouseEvent
    {
        public int Button { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Delta { get; set; }

        public int ScrollDelta { get; set; }

        public bool Handled { get; set; }

        public UIMouseEvent(int button, Vector2 position, Vector2 delta, int scrollDelta)
        {
            Button = button;
            Position = position;
            Delta = delta;
            ScrollDelta = scrollDelta;
        }
    }
}