using EngineLite.Engine.Core;
using EngineLite.Engine.ECS;
using EngineLite.Engine.ECS.Systems;
using EngineLite.Engine.ECS.Systems.UISystems;
using EngineLite.Engine.EngineDebug;
using EngineLite.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace EngineLite
{
    public class GameEngine
    {
        public static GameEngine Instance;
        private GraphicsDeviceManager _graphics;
        public ContentManager Content{get; private set;}
        public GameWindow Window{get; private set;} 
        private bool _needsSceneChange = false;
        public bool IsPaused{get;set;} = false;
        // public bool IsMouseVisible => _game.IsMouseVisible;
        public bool DrawDebug{get;set;} = true;
        public Camera Camera{get; private set;}
        private string _defaultFont;
        public string Version{get; private set;} = "v0.0.1";






        public GameEngine(GameWindow window, GraphicsDeviceManager graphics, ContentManager content, string DefaultFont)
        {
            Window = window;
            _graphics = graphics;
            Content = content;
            _defaultFont = DefaultFont;
            Camera = new Camera(_graphics.GraphicsDevice.Viewport);
            Instance = this;
        }

        public void Load(string settingsPath, Vector2 cameraPos, float cameraZoom)
        {
            Core();
            Managers(settingsPath);
            Debug();
            ECS();
            LoadCamera(cameraPos, cameraZoom);
        }

        private void Core()
        {
            AssetLoader.Init(Content, _graphics.GraphicsDevice, _defaultFont);
        }

        private void Managers(string settingsFile)
        {
            new SettingsManager(settingsFile);
            new WindowManager(Window);
            new ResolutionManager(_graphics, new Resolution(1280, 720, false));
            _graphics.ApplyChanges();
            new SceneManager(SceneChangeCallBack);            
        }

        private void SceneChangeCallBack()
        {
            _needsSceneChange = true;
        }

        private void Debug()
        {
            new OnScreenLog(AssetLoader.DefaultFont);
            new Stats(new Vector2(1000, 10));          
            Stats.Instance.Add("EngineLite Version", GetVersion);
            Stats.Instance.Add("Delta Time", Time.GetDeltaString);
            Stats.Instance.Add("FPS", Time.GetFpsString);
            Stats.Instance.Add("Screen to World", MouseScreenToWorld);
            Stats.Instance.Add("World to Screen", MouseWorldToScreen);
            Stats.Instance.Add("Draw Physics Degug", ShowPhysicsDebug);
            Stats.Instance.Add("Scene", SceneManager.Instance.GetActiveSceneName);
        }

        private void ECS()
        {
            PhysicsWorld.Init();
            new EntityWorld();
            new SystemsManager();

            new SpriteSystem();
            new SpriteSheetSystem();
            new AnimatorSystem();
            new PhysicsDebugDrawSystem();
            new PlayerMoverSystem();
            new CameraFollowSystem();
            new PlayerFacerSystem();     


            new UISystem();
        }

        private void LoadCamera(Vector2 position, float zoom)
        {   
            Camera.Position = position;
            Camera.Zoom = zoom;
        }

        public void Update(GameTime gameTime)
        {
            if(_needsSceneChange)
            {
                SceneManager.Instance.HandleSceneChange();
                _needsSceneChange = false;
                SystemsManager.Instance.Initialize();
            }

            Time.Update(gameTime);
            Input.Update();

            if(IsPaused){return;}

            if(Input.GetKeyDown(Keys.P))
            {
                DrawDebug = !DrawDebug;
            }

            UISystem.Instance.Update();
            PhysicsWorld.Update();
            SystemsManager.Instance.Update();

            if(Input.IsScrolling)
            {
                Camera.Zoom += Input.ScrollWheelDelta * 0.1f;
            }            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);

            SystemsManager.Instance.Draw(spriteBatch);

            spriteBatch.End();            




            //UI / Debug
            spriteBatch.Begin(SpriteSortMode.Immediate);

            OnScreenLog.Instance.Draw(spriteBatch);

            Stats.Instance.Draw(spriteBatch);

            UISystem.Instance.Draw(spriteBatch);

            spriteBatch.End();

        }


        private string GetVersion() => Version;
        private string MouseScreenToWorld() => Camera.ScreenToWorld(Input.MousePos).ToString();
        private string MouseWorldToScreen() => Camera.WorldToScreen(Input.MousePos).ToString();
        private string ShowPhysicsDebug() => DrawDebug.ToString();


    }
}