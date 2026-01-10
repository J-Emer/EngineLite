using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace EngineLite.Engine.Core
{
    public static class AssetLoader
    {
        private static ContentManager _content;
        private static Texture2D _pixel;

        private static readonly Dictionary<string, Texture2D> _textures = new();
        private static readonly Dictionary<string, SpriteFont> _fonts = new();

        public static SpriteFont DefaultFont { get; private set; }

        /// <summary>
        /// Initialize the AssetLoader with content manager, graphics device, and default font
        /// </summary>
        public static void Init(ContentManager content, GraphicsDevice graphicsDevice, string defaultFontName)
        {
            _content = content;

            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            DefaultFont = GetFont(defaultFontName);
        }

        public static Texture2D GetPixel() => _pixel;

        /// <summary>
        /// Returns a Texture2D from Game.Content
        /// </summary>
        /// <param name="name">Name of the Texture</param>
        /// <returns></returns>
        public static Texture2D GetTexture(string name)
        {
            if (!_textures.ContainsKey(name))
                _textures[name] = _content.Load<Texture2D>(name);

            return _textures[name];
        }

        public static SpriteFont GetFont(string name)
        {
            if (!_fonts.ContainsKey(name))
                _fonts[name] = _content.Load<SpriteFont>(name);

            return _fonts[name];
        }
    }
}
