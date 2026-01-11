using EngineLite.Engine.ECS.Systems;

namespace EngineLite.Engine.ECS.Components
{
    public struct OnCollisionComponent
    {
        public Action<CollisionEvent>Callback;
    }
}