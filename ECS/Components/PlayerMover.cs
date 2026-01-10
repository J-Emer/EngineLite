namespace EngineLite.Engine.ECS.Components
{
    public struct PlayerMover
    {
        public float Speed{get;set;} = 5f;
        public float JumpForce{get;set;} = -25f;
        public bool IsGrounded{get;set;} = true;

        public PlayerMover()
        {
        }

    }
}