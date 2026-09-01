using EngineLite.Core;

namespace EngineLite.GameObjects.Components
{
    public class WASDMover : Component
    {
        public float MoveSpeed { get; set; } = 5f;

        public override void Update()
        {
            Transform.Position += Input.GetAxis() * MoveSpeed;
        }
    }
}
