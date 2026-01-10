using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using EngineLite.Engine.Core;

namespace EngineLite.Engine.EngineDebug

{
    public class OnScreenLog
    {
        public static OnScreenLog Instance{get; private set;}
        public Vector2 Position{get;set;} = new Vector2(10, 10);
        public float Yoffset{get;set;} = 20f;
        public SpriteFont Font{get; private set;}
        public Color DrawColor{get;set;} = Color.Black;
        public float MessageLifeTime{get;set;} = 5f;
        private float _elapsed = 0f;
        private List<string> _messages = new List<string>();

        public OnScreenLog(SpriteFont _font)
        {
            Font = _font;
            Instance = this;
        }

        public void Log(string _message)
        {
            _messages.Add(_message);
        }

        public void Draw(SpriteBatch _spritebatch)
        {
            if(_messages.Count == 0){return;}

            _elapsed += Time.DeltaTime;

            if(_elapsed >= MessageLifeTime)
            {
                _messages.RemoveAt(0);
                _elapsed = 0f;
            }

            float _y = Position.Y;

            foreach (var item in _messages)
            {
                _spritebatch.DrawString(
                    Font,
                    item,
                    new Vector2(Position.X, _y),
                    DrawColor
                );

                _y += Yoffset;
            }
        }

        public void Clear()
        {
            _messages.Clear();
        }
    }
}