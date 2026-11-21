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
using EngineLiteTest.Managers;

namespace EngineLite
{
    public class Engine
    {
        public string Version{get; private set;} = "V 0.0.1";
        public static Engine Instance{get; private set;}
        private Game Game;
        public GraphicsDeviceManager Graphics{get; private set;}
        public DebugScreen DebugScreen{get; private set;}
        public SceneManager SceneManager{get; private set;}
        public ResolutionManager ResolutionManager{get; private set;}


        public bool IsPaused{get;set;} = false;
        public bool MouseVisible
        {
            get => Game.IsMouseVisible;
            set => Game.IsMouseVisible = value;
        }
        public bool ShowDebug{get;set;} = true;

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

            ResolutionManager = new ResolutionManager(Graphics, new Resolution(1280, 720, false));
            SceneManager = new SceneManager(scenes);

            //todo: pass the SceneManager to the constrtuctor -> if order of 
            //operations (world created before SceneManager) changes could throw a null ref exception
            World = new World();
            SceneManager.BeforeSceneChange += BeforeSceneChange;

            CollisionSystem = new CollisionSystem(64);

            new GameUIRenderer(Game, "font"); //---this is a singleton for now
            
            LightSystem = new PenumbraComponent(Game);
            LightSystem.Initialize();

            DebugScreen = new DebugScreen();
            DebugScreen.Position = new Vector2(1050, 10);
            DebugScreen.Add("Version", () => Version.ToString());
            DebugScreen.Add("FPS", () => Time.FPS.ToString());
            DebugScreen.Add("Scene", () => SceneManager.ActiveSceneName);
            DebugScreen.Add("Gameobjects", () => World.GameObjectsCount.ToString());
            DebugScreen.Add("Cam Pos - World", () => MainCamera.Instance.Position.ToString());
            DebugScreen.Add("Mouse - World", () => MainCamera.Instance.ScreenToWorld(Input.MousePosition).ToString());

            new MainCamera(Game.GraphicsDevice.Viewport);
            
            SceneManager.LoadScene(firstScene);

            Logger.Log(this, Version);
        }

        private void BeforeSceneChange()
        {
            World.Clear();
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

            LightSystem.Transform = MainCamera.Instance.GetViewMatrix();
            LightSystem.BeginDraw();

            Game.GraphicsDevice.Clear(Color.CornflowerBlue);

            //StatManager
            spriteBatch.Begin();
            DebugScreen.Draw(spriteBatch);
            spriteBatch.End();

            //Game
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, MainCamera.Instance.GetViewMatrix());
            World.Draw(spriteBatch);
            
            CollisionSystem.DrawDebug(spriteBatch);

            spriteBatch.End();            

            LightSystem.Draw(Time.GameTime);


            //Game UI
            spriteBatch.Begin();
            GameUIRenderer.Instance.Draw(spriteBatch);
            spriteBatch.End();

        }
    }
}