using System;
using EngineLite.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components.UI
{
    public class Button : UIComponent
    {
        public SpriteFont Font{get;set;} = AssetLoader.DefaultFont;
        public string Text{get;set;} = "Button";
        public Color FontColor{get;set;} = Color.Black;
        public Color NormalColor{get;set;} = Color.Transparent;
        public Color HighlightColor{get;set;} = Color.LightGray;
        private bool PMouse = false;
        private bool CMouse = false;


        public Action OnClick;

        public Button()
        {
            Scale = new Vector2(100, 30);
            BackgroundColor = NormalColor;
        }

        public override void Update()
        {
            PMouse = CMouse;
            CMouse = SourceRect.Contains(Input.MousePos);

            //enter
            if(!PMouse && CMouse)
            {
                BackgroundColor = HighlightColor;
            }

            //clicked
            if(CMouse && Input.GetMouseButtonDown(Input.MouseButton.Left))
            {
                OnClick?.Invoke();
            }

            //exit
            if(PMouse && !CMouse)
            {
                BackgroundColor = NormalColor;
            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 center = new Vector2(SourceRect.Center.X, SourceRect.Center.Y);
            Vector2 halfText = Font.MeasureString(Text) / 2;

            spriteBatch.DrawString(Font, Text, center - halfText, FontColor);
        }
    }    
}