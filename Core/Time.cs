using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EngineLite.Core
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }
        public static int FPS { get; private set; }
        public static GameTime GameTime{get; private set;}

        private static int frameCount;

        private static float elapsedTime;

        public static void Update(GameTime time)
        {
            GameTime = time;
            
            DeltaTime = (float)time.ElapsedGameTime.TotalSeconds;

            frameCount += 1;
            elapsedTime += DeltaTime;

            if (elapsedTime >= 1f)
            {
                FPS = frameCount;
                frameCount = 0;
                elapsedTime = 0;
            }
        }

        public static string GetFPS_String() => FPS.ToString();
        public static string GetDeltaTime_String() => DeltaTime.ToString();
    }
}
