using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLite.Managers
{
    public class SceneManager
    {
        private List<Scene> _scenes = new();
        private Scene _activeScene = null;
        private Scene _nextScene = null;
        private bool _needsSceneChange = false;

        public Action BeforeSceneChange;
        public Action AfterSceneChange;

        public string ActiveSceneName
        {
            get
            {
                if(_activeScene == null)
                {
                    return "No Scene Loaded";
                }

                return _activeScene.Name;
            }
        }

        public SceneManager(List<Scene> scenes)
        {
            _scenes = scenes;
        }
        public void LoadScene(string name)
        {
            _nextScene = _scenes.FirstOrDefault(x => x.Name == name);

            if(_nextScene == null)
            {
                throw new Exception($"Scene: {name} could not be found");
            }

            _needsSceneChange = true;
        }

        public void Update()
        {
            if(_needsSceneChange)
            {
                ChangeScene();
            }
        }

        private void ChangeScene()
        {
            BeforeSceneChange?.Invoke();

            if(_activeScene != null)
            {
                _activeScene.UnLoad();
            }

            _activeScene = _nextScene;

            _activeScene.Load();

            AfterSceneChange?.Invoke();

            _needsSceneChange = false;
        }
    }
}
