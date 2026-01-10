using System.Collections.Generic;

namespace EngineLite.Engine.IO.MapImporter
{
    public class Layer
    {
        public string name{get;set;}
        public bool collider{get;set;}

        public List<Tile> tiles = new List<Tile>();
    }

    public class Tile
    {
        public int id{get;set;}
        public int x{get;set;}
        public int y{get;set;}
        public Dictionary<string, string> attributes { get; set; }
    }
}