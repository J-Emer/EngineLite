using System.IO;
using EngineLite.Engine.ECS;
using Newtonsoft.Json;


namespace EngineLite.Engine.IO
{
    public class SceneSaver
    {
        private string _saveFile;

        public SceneSaver(string saveFile)
        {
            _saveFile = saveFile;
        }

        public void Save()
        {
            SceneData _data = new SceneData(
                EntityWorld.Instance.GetAllEntities(),
                EntityWorld.Instance.GetAllComponents()
            );

            using (StreamWriter writer = new StreamWriter(_saveFile))
            {
                writer.Write(JsonConvert.SerializeObject(_data, Formatting.Indented, new JsonSerializerSettings
                                                                                        {
                                                                                            TypeNameHandling = TypeNameHandling.Auto
                                                                                        }));
            }
        }
    }    
}