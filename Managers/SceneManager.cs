using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineLite.Engine.Managers
{
    public class SceneManager
    {
        public static SceneManager Instance { get; private set; }
        private readonly List<Scene> _scenes = new List<Scene>();
        private Scene _activeScene;
        private Scene _nextScene;
        private readonly Action _needsSceneChangeCallBack;
        public event Action BeforeSceneLoads;
        public event Action AfterSceneLoads;

        public SceneManager(Action needsSceneChangeCallBack)
        {          
            _needsSceneChangeCallBack = needsSceneChangeCallBack;
            Instance = this;
        }

        public void Add(Scene scene) => _scenes.Add(scene);

        public void LoadScene(string sceneName)
        {
            _nextScene = _scenes.FirstOrDefault(x => x.Title == sceneName);
            if (_nextScene != null)
            {
                _needsSceneChangeCallBack?.Invoke();
            }
        }

        public void HandleSceneChange()
        {
            if (_nextScene == null) return;

            BeforeSceneLoads?.Invoke();

            _activeScene?.Unload();

            _activeScene = _nextScene;
            _nextScene = null;
            _activeScene.Load();

            AfterSceneLoads?.Invoke();
        }

        public string GetActiveSceneName() => _activeScene?.Title ?? string.Empty;
    }
}
