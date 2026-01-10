using System;
using System.IO;
using EngineLite.Engine.ECS;
using Newtonsoft.Json;


namespace EngineLite.Engine.IO
{
    public class SceneLoader
    {
        private string _saveFile;

        public SceneLoader(string saveFile)
        {
            _saveFile = saveFile;
        }

        public void Load()
        {
            SceneData _data;

            using (StreamReader reader = new StreamReader(_saveFile))
            {
                _data = JsonConvert.DeserializeObject<SceneData>(reader.ReadToEnd(), new JsonSerializerSettings
                                                                                        {
                                                                                            TypeNameHandling = TypeNameHandling.Auto
                                                                                        });
            }

            EntityWorld.Instance.LoadFromFile(_data);
        }
    }    
}