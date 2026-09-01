using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineLite.Core;
using EngineLite.UI.Controls;
using EngineLite.UI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.UI
{
    public class GameUIRenderer : IDisposable
    {
        public static GameUIRenderer Instance { get; private set; }

        private Game Game;
        
        private List<Control> controls = new List<Control>();
        public int ControlCount => controls.Count;

        private List<ContainerControl> overlayControls = new List<ContainerControl>();
        public bool HasOverlays => overlayControls.Count > 0;
        public Texture2D Pixel { get; private set; }
        public SpriteFont DefaultFont { get; set; }
        private MouseInteractions mouseInteractions;

        public GameUIRenderer(Game game, string defaultFontName)
        {
            if(Instance != null)
            {
                throw new Exception("Desktop Instance already exists. Dispose of previouse Desktop before creating a new one");
            }

            Instance = this;

            Game = game;

            Pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            DefaultFont = game.Content.Load<SpriteFont>(defaultFontName);

            mouseInteractions = new MouseInteractions();

            Engine.Instance.SceneManager.BeforeSceneChange += BeforeSceneChanged;
        }

        private void BeforeSceneChanged()
        {
            Clear();
        }

        ~GameUIRenderer()
        {
            Dispose();
        }
        public void Add(Control _control) => controls.Add(_control);
        public void Remove(Control _control) => controls.Remove(_control);
        public Control Find(string name)
        {
            return controls.FirstOrDefault(x => x.Name == name);
        }
        public T Find<T>(string name) where T : Control
        {
            return (T)controls.FirstOrDefault(x => x.Name == name);
        }
        public void Clear()
        {
            controls.Clear();
            overlayControls.Clear();
        }

        public void Update()
        {
            //handle input
            mouseInteractions.HandleInteraction(controls, overlayControls);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                controls[i].Draw(spriteBatch);
            }

            //overlays
            for (int i = 0; i < overlayControls.Count; i++)
            {
                overlayControls[i].Draw(spriteBatch);
            }
        }

        public void Dispose()
        {
            controls.Clear();
            overlayControls.Clear();
            Instance = null;
        }



        //-----------------------------Overlay------------------------//

        public void AddOverlay(ContainerControl control)
        {
            if (!overlayControls.Contains(control))
            {
                overlayControls.Add(control);
            }
        }

        public void RemoveOverlay(ContainerControl control)
        {
            if (overlayControls.Contains(control))
            {
                overlayControls.Remove(control);
            }
        }

        public void ClearOverlays()
        {
            overlayControls.Clear();
        }


    }
}
