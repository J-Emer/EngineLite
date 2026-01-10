using Microsoft.Xna.Framework;

namespace EngineLite.Engine.ECS.Components
{
    public struct CameraTarget
    {
        public bool LockYAxis{get;set;} = true;
        public float FollowSpeed { get; set; } = 8f; 
        public Vector2 Offset{get;set;} = Vector2.Zero;

        public CameraTarget()
        {
        }

    }
}
