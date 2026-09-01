using System;
using System.Collections.Generic;
using EngineLite.Core;
using EngineLite.GameObjects;
using EngineLite.GameObjects.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EngineLite.GameObjects.Components
{
    public class Animator : Component
    {
        public List<Animation> Animations{get;set;}
        private int Index = 0;
        public Animation ActiveAnimation
        {
            get
            {
                if (Animations == null || Animations.Count == 0)
                    return null;

                if (Index < 0 || Index >= Animations.Count)
                    return null;

                return Animations[Index];
            }
        }
        private Sprite sprite;
        private bool _isPaused = false;


        public Animator(List<Animation> animations)
        {
            Animations = animations;
        }
        public override void Start()
        {
            sprite = GameObject.GetComponent<Sprite>();
            sprite.SourceRectangle = ActiveAnimation.ActiveFrame;
        }
        public override void Update()
        {
            if(_isPaused){return;}

            ActiveAnimation.timer += Time.DeltaTime;

            if(ActiveAnimation.timer >= ActiveAnimation.FrameRate)
            {
                ActiveAnimation.timer = 0;
                ActiveAnimation.FrameIndex += 1;

                if(ActiveAnimation.FrameIndex > ActiveAnimation.Frames.Count -1)
                {
                    ActiveAnimation.FrameIndex = 0;
                }

                sprite.SourceRectangle = ActiveAnimation.ActiveFrame;
            }
        }
        public void Play(string name)
        {
            if (ActiveAnimation.Name == name){return;}

            for (int i = 0; i < Animations.Count; i++)
            {
                if (Animations[i].Name != name)
                {
                    continue;                    
                }

                Index = i;

                ActiveAnimation.FrameIndex = 0;
                ActiveAnimation.timer = 0f;
                _isPaused = false;

                if (sprite != null)
                {
                    sprite.SourceRectangle = ActiveAnimation.ActiveFrame;                    
                }

                return;
            }
        }

        public void Restart()
        {
            ActiveAnimation.FrameIndex = 0;
            ActiveAnimation.timer = 0f;  
        }

        public void Pause()
        {
            _isPaused = !_isPaused;
        }
    }

    public class Animation
    {
        public string Name{get;set;}
        public List<Rectangle> Frames{get;set;}
        public int FrameIndex{get;set;} = 0;
        public Rectangle ActiveFrame => Frames[FrameIndex];
        public float FrameRate{get;set;}
        public float timer{get;set;} = 0;
        
        public Animation(string name, List<Rectangle> frames, float frameRate)
        {
            Name = name;
            Frames = frames;
            FrameRate = frameRate;
        }
    }
}