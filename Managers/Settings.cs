using Microsoft.Xna.Framework;

namespace EngineLite.Engine.Managers
{
    public class Settings
    {
        public string GameTitle { get; set; } = "My Game";
        public Resolution DefaultResolution { get; set; } = new Resolution(1280, 720, false);
        public Color ClearColor { get; set; } = Color.CornflowerBlue;
        public Color AmbientColor { get; set; } = new Color(0, 0, 0, 150);
        public string FirstScene { get; set; } = "Demo";
        public Vector2 GravityDirection { get; set; } = new Vector2(0, 10);

        public override string ToString()
        {
            return $"Settings: GameTitle: {GameTitle} | DefaultResolution: {DefaultResolution} | ClearColor: {ClearColor} | AmbientColor: {AmbientColor}";
        }
    }
}
