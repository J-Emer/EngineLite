using System;
using EngineLite.Engine.Core;
using EngineLite.Engine.ECS.Components;

namespace EngineLite.Engine.ECS.Systems
{
    public class AnimatorSystem : UpdateSystem
    {
        public override void Update()
        {
            foreach (var entityId in EntityWorld.Instance.GetEntitiesWithComponents<Sprite, Animator>())
            {
                EntityWorld.Instance.GetComponent(entityId, out Sprite sprite);
                EntityWorld.Instance.GetComponent(entityId, out Animator animator);

                Animation anim = animator.Animations[animator.ActiveAnimation];
                int animationIndex = animator.ActiveAnimation;

                anim.Timer += Time.DeltaTime;

                if (anim.Timer >= anim.FrameRate)
                {
                    anim.Timer = 0;
                    anim.ActiveFrame++;

                    if (anim.ActiveFrame >= anim.Frames.Count)
                        anim.ActiveFrame = 0;

                    sprite.Texture = AssetLoader.GetTexture(anim.Frames[anim.ActiveFrame]);
                    //sprite.SourceRect = anim.Frames[anim.ActiveFrame];

                    EntityWorld.Instance.SetComponent(entityId, sprite);//only reset if the animation frame changes
                }

                animator.Animations[animationIndex] = anim;//reset every frame
                EntityWorld.Instance.SetComponent(entityId, animator);//reset every frame
            }
        }


        public void DoWork()
        {
            Console.WriteLine("---this is the AnimatorSystem:DoWord()");
        }
    }
}