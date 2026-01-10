using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public abstract class UIComponent
    {
        public Vector2 Position
        {
            get => _position;
            set
            {
                if(_position != value)
                {
                    _position = value;
                    HandleDirty();
                }
            }
        }
        private Vector2 _position = Vector2.Zero;
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                if(_scale != value)
                {
                    _scale = value;
                    HandleDirty();
                }
            }
        }
        private Vector2 _scale = Vector2.Zero;
        public Texture2D Background{get;set;} = AssetLoader.GetPixel();
        public Color BackgroundColor{get;set;} = Color.White;
        protected Rectangle SourceRect = new Rectangle();
        protected Vector2 Center => new Vector2(SourceRect.Center.X, SourceRect.Center.Y);


        private void HandleDirty()
        {
            SourceRect = new Rectangle(
                                    (int)Position.X,
                                    (int)Position.Y,
                                    (int)Scale.X,
                                    (int)Scale.Y
                                );
        }
        public virtual void Update(){}
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, SourceRect, BackgroundColor);
        }



    }




}