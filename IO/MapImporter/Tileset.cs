using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.IO.MapImporter
{
    public class TileSet
    {
        public Texture2D Texture{get;set;}
        public int TileSize{get;set;}

        public TileSet(Texture2D texture, int tileSize)
        {
            Texture = texture;
            TileSize = tileSize;
        }

        public Rectangle GetSourceRect(int tileId)
        {
            int tilesPerRow = Texture.Width / TileSize;

            int x = tileId % tilesPerRow;
            int y = tileId / tilesPerRow;

            return new Rectangle(
                x * TileSize,
                y * TileSize,
                TileSize,
                TileSize
            );
        }
    }
}