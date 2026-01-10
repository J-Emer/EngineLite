using Microsoft.Xna.Framework;

namespace EngineLite.Engine.Core
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }
        public static float FPS { get; private set; }
        public static GameTime Current { get; private set; }

        public static void Update(GameTime gameTime)
        {
            Current = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FPS = DeltaTime > 0f ? 1f / DeltaTime : 0f;
        }

        /// <summary>
        /// Returns FPS as a formatted string
        /// </summary>
        public static string GetFpsString() => FPS.ToString("0.00");

        /// <summary>
        /// Returns Delta Time as a formatted string
        /// </summary>
        public static string GetDeltaString() => DeltaTime.ToString("0.000");
    }
}
