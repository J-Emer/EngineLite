
namespace EngineLite.Managers
{
    public abstract class Scene
    {
        public string Name { get; private set; } = "Default_Scene";

        public Scene(string name)
        {
            Name = name;
        }
        public abstract void Load();
        public abstract void UnLoad();

    }
}
