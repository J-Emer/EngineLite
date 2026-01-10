using EngineLite.Engine.ECS.Components;
using EngineLite.Engine.EngineDebug;
using EngineLite.Engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;

namespace EngineLite.Engine.ECS.Systems
{
    public class PhysicsDebugDrawSystem : DrawSystem
    {

        public override void Draw(SpriteBatch _spritebatch)
        {
            if(!GameEngine.Instance.DrawDebug){return;}

            foreach (int entityId in EntityWorld.Instance.GetEntitiesWithComponent<PhysicsBody>())
            {
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physics);

                DrawBody(_spritebatch, physics.Body);
            }
        }

        private void DrawBody(SpriteBatch spriteBatch, Body body)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                if (fixture.Shape.ShapeType != ShapeType.Polygon)
                    continue;

                DrawPolygon(spriteBatch, body, (PolygonShape)fixture.Shape);
            }
        }

        private void DrawPolygon(SpriteBatch spriteBatch, Body body, PolygonShape polygon)
        {
            var verts = polygon.Vertices;
            int count = verts.Count;

            for (int i = 0; i < count; i++)
            {
                Vector2 localA = verts[i];
                Vector2 localB = verts[(i + 1) % count];

                Vector2 worldA = body.GetWorldPoint(localA);
                Vector2 worldB = body.GetWorldPoint(localB);

                Vector2 pixelA = ConvertUnits.ToDisplayUnits(worldA);
                Vector2 pixelB = ConvertUnits.ToDisplayUnits(worldB);

                DebugDraw.DrawLine(spriteBatch, pixelA, pixelB, Color.White);
            }
        }
    }
}
