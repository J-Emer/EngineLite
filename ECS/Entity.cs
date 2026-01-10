namespace EngineLite.Engine.ECS
{
    public struct Entity
    {
        public int Id { get; private set; }
        public Entity(int id)
        {
            Id = id;
        }
    }
}