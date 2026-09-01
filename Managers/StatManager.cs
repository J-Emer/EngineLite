using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EngineLite.Managers
{
    public class StatManager
    {
        private Dictionary<string, Func<string>> stats = new Dictionary<string, Func<string>>();
        public Vector2 StartPosition { get; set; } = new Vector2(10, 10);
        public float YOffset { get; set; } = 20f;
        public Color FontColor { get; set; } = Color.Black;

        public bool ShowStats = true;
        public SpriteFont Font { get; set; }


        public StatManager(SpriteFont font)
        {
            Font = font;
        }
        public void Add(string name, Func<string> callback)
        {
            if (stats.ContainsKey(name))
            {
                return;
            }
            stats.Add(name, callback);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (!ShowStats) { return; }

            int i = 0;

            foreach (var item in stats)
            {
                string _message = $"{item.Key}: {item.Value.Invoke()}";
                float _yPos = (i * YOffset) + StartPosition.Y;

                spriteBatch.DrawString(Font, _message, new Vector2(StartPosition.X, _yPos), FontColor);

                i += 1;
            }
        }
    }
}
