using System.Text;
using EngineLite;
using EngineLite.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLiteTest.Managers
{
    public class DebugScreen : IDisposable
    {
        private readonly List<(string Label, Func<string> Callback)> _stats = new();
        private readonly StringBuilder _sb = new();
        private bool _canDraw;

        public Vector2 Position { get; set; } = new Vector2(10, 10);
        public int YOffset { get; set; } = 20;
        public Color FontColor { get; set; } = Color.White;

        public DebugScreen()
        {
            _canDraw = Engine.Instance.ShowDebug;
            Engine.Instance.OnDebugStateChanged += DebugStateChanged;
        }

        private void DebugStateChanged(bool value) => _canDraw = value;

        public void Add(string label, Func<string> callback)
        {
            int index = _stats.FindIndex(s => s.Label == label);
            if (index >= 0)
                _stats[index] = (label, callback);   // overwrite instead of throwing
            else
                _stats.Add((label, callback));
        }

        public bool Remove(string label) => _stats.RemoveAll(s => s.Label == label) > 0;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_canDraw) return;

            Vector2 pos = Position;

            // index loop so Add/Remove during Draw can't invalidate an enumerator
            for (int i = 0; i < _stats.Count; i++)
            {
                var (label, callback) = _stats[i];

                string message;
                try { message = callback?.Invoke(); }
                catch (Exception ex) { message = $"<error: {ex.GetType().Name}>"; }

                _sb.Clear();
                _sb.Append(label).Append(": ").Append(message);

                spriteBatch.DrawString(AssetLoader.DefaultFont, _sb, pos, FontColor);
                pos.Y += YOffset;
            }
        }

        public void Dispose()
        {
            Engine.Instance.OnDebugStateChanged -= DebugStateChanged;
        }
    }
}