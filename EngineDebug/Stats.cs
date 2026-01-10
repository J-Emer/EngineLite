using System;
using System.Collections.Generic;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace EngineLite.Engine.EngineDebug
{
    public class Stats
    {
        public static Stats Instance;
        
        private SpriteFont _font;
        private Vector2 _startPos;
        public float YOffset = 20f;
        public bool ShowStats{get;set;} = true; 

        private Dictionary<string, Func<string>> _messages = new Dictionary<string, Func<string>>();

        public Stats(Vector2 _startPos)
        {
            this._font = AssetLoader.DefaultFont;
            this._startPos = _startPos;
            Instance = this;
        }
        /// <summary>
        /// Adds a new "Stat" to the stats list 
        /// </summary>
        public void Add(string _title, Func<string> _callBack) => _messages.Add(_title, _callBack);
        public void Remove(string _title) => _messages.Remove(_title);
        public void Draw(SpriteBatch _spritebatch)
        {
            if(!ShowStats){return;}

            float yPos = _startPos.Y;

            foreach (var item in _messages)
            {
                string _message = item.Key + ": " + item.Value.Invoke();
                Vector2 _pos = new Vector2(_startPos.X, yPos);

                _spritebatch.DrawString(_font, _message, _pos, Color.Black);

                yPos += YOffset;
            }

        }
    }
}

