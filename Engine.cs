using System;
using System.Collections.Generic;
using EngineLite.Core;
using EngineLite.Managers;
using EngineLite.UI;
using EngineLite.Util;
using EngineLite.GameObjects;
using EngineLite.GameObjects.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace EngineLite
{
    public class Engine
    {
        public string Version{get; private set;} = "V 0.0.1";
        public static Engine Instance{get; private set;}
        private Game Game;
        public GraphicsDeviceManager Graphics{get; private set;}
        public StatManager StatManager{get; private set;}
        public SceneManager SceneManager{get; private set;}
        public ResolutionManager ResolutionManager{get; private set;}


        public bool IsPaused{get;set;} = false;
        public bool MouseVisible
        {
            get => Game.IsMouseVisible;
            set => Game.IsMouseVisible = value;
        }
        public bool ShowDebug{get; private set;} = false;
        public bool ShowStats
        {
            get => StatManager.ShowStats;
            set => StatManager.ShowStats = value;
        }

        public World World{get; private set;}
        public CollisionSystem CollisionSystem{get; private set;}
        // public GameUIRenderer GameUIRenderer{get; private set;} //this is a singleton for now
        public PenumbraComponent LightSystem{get; private set;}
        public Color AmbientColor
        {
            get => LightSystem.AmbientColor;
            set => LightSystem.AmbientColor = value;
        } 



        public event Action<bool> OnDebugStateChanged;




        public Engine(Game _game, GraphicsDeviceManager _graphicsManager) : base()
        {
            Game = _game;
            Graphics = _graphicsManager;

            if(Instance != null)
            {
                throw new System.Exception("Only one instance of Engine is permitted");
            }

            Instance = this;
        }


        public void LoadContent(string defaultFont, List<Scene> scenes, string firstScene)
        {
            AssetLoader.Init(Game.Content, Game.GraphicsDevice, defaultFont);
            ShapeRenderer.Init(Game.GraphicsDevice);

            ResolutionManager = new ResolutionManager(Graphics, new Resolution(1280, 720, false));
            SceneManager = new SceneManager(scenes);

            //todo: pass the SceneManager to the constrtuctor -> if order of 
            //operations (world created before SceneManager) changes could throw a null ref exception
            World = new World();
            CollisionSystem = new CollisionSystem(64);
            new GameUIRenderer(Game, "font");
            
            LightSystem = new PenumbraComponent(Game);
            LightSystem.Initialize();


            StatManager = new StatManager(AssetLoader.DefaultFont);
            StatManager.Add("FPS", () => Time.FPS.ToString());
            StatManager.Add("Scene", () => SceneManager.ActiveSceneName);
            StatManager.Add("Gameobjects", () => "???");

            new MainCamera(Game.GraphicsDevice.Viewport);
            
            SceneManager.LoadScene(firstScene);

            Logger.Log(this, Version);
        }

        public void ToggleDebug()
        {
            ShowDebug = !ShowDebug;
            LightSystem.Debug = ShowDebug;
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.Exit();                
            }

            Time.Update(gameTime);
            Input.Update();

            if(Input.GetKeyDown(Keys.Q))
            {
                Logger.Log(this, "Toggle Editor Controls");
                ShowDebug = !ShowDebug;
                OnDebugStateChanged?.Invoke(ShowDebug);
            }

            if(IsPaused){return;}

            SceneManager.Update();
            World.Update();
            CollisionSystem.Update();
            GameUIRenderer.Instance.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            //StatManager
            spriteBatch.Begin();
            StatManager.Draw(spriteBatch);
            spriteBatch.End();


            LightSystem.Transform = MainCamera.Instance.GetViewMatrix();
            LightSystem.BeginDraw();

            Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Game
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, MainCamera.Instance.GetViewMatrix());
            World.Draw(spriteBatch);
            
            if(ShowDebug)
            {
                CollisionSystem.DrawDebug(spriteBatch);
            }

            spriteBatch.End();            

            LightSystem.Draw(Time.GameTime);


            //Game UI
            spriteBatch.Begin();
            GameUIRenderer.Instance.Draw(spriteBatch);
            spriteBatch.End();

        }
    }
}