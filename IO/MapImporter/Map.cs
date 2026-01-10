using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EngineLite.Engine.IO.MapImporter
{
    public class Map
    {
        public int tileSize{get;set;}
        public int mapWidth{get;set;}
        public int mapHeight{get;set;}
        public List<Layer> layers = new List<Layer>();

    }
}