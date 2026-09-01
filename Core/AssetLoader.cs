using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace EngineLite.Core
{
    public static class AssetLoader
    {
        private static ContentManager content;
        public static Texture2D Pixel { get; private set; }
        public static SpriteFont DefaultFont { get; private set; }

        public static void Init(ContentManager _content, GraphicsDevice _graphics, string _defaultFontName)
        {
            Pixel = new Texture2D(_graphics, 1, 1);
            Pixel.SetData(new[] { Color.White });

            content = _content;

            DefaultFont = GetFont(_defaultFontName);
        }

        public static Texture2D GetTexture(string name) => content.Load<Texture2D>(name);
        public static SpriteFont GetFont(string name) => content.Load<SpriteFont>(name);
        public static Song GetSong(string name) => content.Load<Song>(name);
        public static SoundEffect GetSFX(string name) => content.Load<SoundEffect>(name);
        public static Effect GetEffect(string name) => content.Load<Effect>(name);

    }
}
