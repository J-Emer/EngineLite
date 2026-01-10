using System;
using System.IO;
using EngineLite.Engine.Core;
using EngineLite.Engine.ECS;
using EngineLite.Engine.ECS.Components;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace EngineLite.Engine.IO.MapImporter
{
    public class Importer
    {
        private string _mapFile;

        public Importer(string mapfile)
        {
            _mapFile = mapfile;
        }

        public void Load()
        {
            Map _data;

            using (StreamReader reader = new StreamReader(_mapFile))
            {
                _data = JsonConvert.DeserializeObject<Map>(reader.ReadToEnd(), new JsonSerializerSettings
                                                                                        {
                                                                                            TypeNameHandling = TypeNameHandling.Auto
                                                                                        });
            }

            TileSet _tileSet = new TileSet(AssetLoader.GetTexture("spritesheet"), 16);

            Convert(_data, _tileSet);
        }

        public static void Convert(Map map, TileSet tileset)
        {
            foreach (var layer in map.layers)
            {
                foreach (var tile in layer.tiles)
                {
                    var entity = EntityWorld.Instance.CreateEntity();

                    // 🧭 Position
                    EntityWorld.Instance.AddComponent(entity, new Transform
                    {
                        Position = new Vector2(tile.x * map.tileSize,tile.y * map.tileSize)
                    });

                    // 🎨 Sprite
                    EntityWorld.Instance.AddComponent(entity, new SpriteSheet
                    {
                        Texture = tileset.Texture,
                        SourceRectangle = tileset.GetSourceRect(tile.id)
                    });

                    // Ground Tiles
                    if (tile.attributes != null && tile.attributes.TryGetValue("Ground", out var ground))
                    {
                        EntityWorld.Instance.AddComponent(entity, new Tag
                        {
                            TagName = "Ground"  
                        });

                        EntityWorld.Instance.AddComponent(entity, new PhysicsBody()
                        {
                            BodyType = nkast.Aether.Physics2D.Dynamics.BodyType.Static,
                        });

                        EntityWorld.Instance.AddComponent(entity, new BoxCollider(new Vector2(16,16)));
                    }

                    //Objects - bottles
                    if (tile.attributes != null && tile.attributes.TryGetValue("Objects", out var asdf))
                    {
                        EntityWorld.Instance.AddComponent(entity, new PhysicsBody()
                        {
                            BodyType = nkast.Aether.Physics2D.Dynamics.BodyType.Dynamic,
                        });

                        EntityWorld.Instance.AddComponent(entity, new BoxCollider(new Vector2(16,16)));
                    }

                }
            }
        }        
    }
}