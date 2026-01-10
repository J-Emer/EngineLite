using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.Engine.ECS.Components
{
    public struct Animator
    {
        public List<Animation> Animations;

        public int ActiveAnimation;

        public Animator(int activeAnimation, List<Animation> animations)
        {
            ActiveAnimation = activeAnimation;
            Animations = animations;
        }
    }


    public struct Animation
    {
    public string Name;
    // public List<Texture2D> Frames;
    public List<string> Frames;
    public float FrameRate;
    public float Timer;
    public int ActiveFrame;
    }
}