using EngineLite.Engine.Core;
using EngineLite.Engine.ECS.Components;
using EngineLite.Engine.EngineDebug;
using EngineLite.Engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;

namespace EngineLite.Engine.ECS.Systems
{

    public struct RayLineDebug
    {
        public Vector2 Start;
        public Vector2 End;

        public RayLineDebug(Vector2 _start, Vector2 _end)
        {
            Start = _start;
            End = _end;
        }
    }


    public class PhysicsDebugDrawSystem : DrawSystem
    {

        private List<RayLineDebug> _rays = new List<RayLineDebug>();

        public void AddRay(Vector2 _start, Vector2 _end)
        {
            _rays.Add(new RayLineDebug(_start, _end));
        }

        public override void Draw(SpriteBatch _spritebatch)
        {
            if(!GameEngine.Instance.DrawDebug){return;}

            foreach (int entityId in EntityWorld.Instance.GetEntitiesWithComponent<PhysicsBody>())
            {
                EntityWorld.Instance.GetComponent(entityId, out PhysicsBody physics);

                DrawBody(_spritebatch, physics.Body);
            }

            foreach (var item in _rays)
            {
                Vector2 edge = item.End - item.Start;
                float angle = (float)Math.Atan2(edge.Y, edge.X);

                _spritebatch.Draw(
                    AssetLoader.GetPixel(),
                    position: item.Start,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: angle,
                    origin: Vector2.Zero,
                    scale: new Vector2(edge.Length(), 3),
                    effects: SpriteEffects.None,
                    layerDepth: 0.2f
                );
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
