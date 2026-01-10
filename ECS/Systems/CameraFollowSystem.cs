using EngineLite.Engine.ECS.Components;
using Microsoft.Xna.Framework;
using EngineLite.Engine.Core;

namespace EngineLite.Engine.ECS.Systems
{
    public class CameraFollowSystem : UpdateSystem
    {


        public override void Update()
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<CameraTarget, Transform>())
            {
                EntityWorld.Instance.GetComponent(entityId, out CameraTarget target);
                EntityWorld.Instance.GetComponent(entityId, out Transform transform);

                Vector2 desiredPos = target.Offset + transform.Position;

                float t = target.FollowSpeed * Time.DeltaTime;

                GameEngine.Instance.Camera.Position = Vector2.Lerp(GameEngine.Instance.Camera.Position, desiredPos, t);

                break;
            }
        }
    }
}
