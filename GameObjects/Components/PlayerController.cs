using System;
using System.Collections.Generic;
using EngineLite.Core;
using EngineLite.Util;
using EngineLite.GameObjects;
using EngineLite.GameObjects.Components;
using EngineLite.GameObjects.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EngineLite;

namespace EngineLite.GameObjects.Components
{
    public class PlayerController : Component
    {
        public float MoveSpeed{get;set;} = 5f;
        private Animator animator;
        private Sprite sprite;
        private BoxCollider boxCollider;
        private bool _isFacingRight = true;


        public override void Start()
        {
            boxCollider = GameObject.GetComponent<BoxCollider>();
            animator = GameObject.GetComponent<Animator>();
            sprite = GameObject.GetComponent<Sprite>();

            boxCollider.OnCollisionEnter += CollisonEnter;
            boxCollider.OnCollisionStay += CollisonStay;
            boxCollider.OnCollisionExit += CollisonExit;

        }

        private void CollisonExit(CollisionInfo info)
        {
            Logger.Log($"Exit: {info.GameObject.Name}");
        }

        private void CollisonStay(CollisionInfo info)
        {
            Logger.Log($"Stay: {info.GameObject.Name}");
        }

        private void CollisonEnter(CollisionInfo info)
        {
            Logger.Log($"Enter: {info.GameObject.Name}");
        }

        public override void Update()
        {
            Vector2 movement = Input.GetAxis();

            Transform.Position += movement * MoveSpeed;

            if (movement.X > 0)
            {
                _isFacingRight = true;
                animator.Play("Run");
            }
            else if (movement.X < 0)
            {
                _isFacingRight = false;
                animator.Play("Run");
            }
            else if(movement.X == 0)
            {
                animator.Play("Idle");
            }

            sprite.Effects = _isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;



            if(Input.GetMouseDown(0))
            {
                Vector2 end = MainCamera.Instance.ScreenToWorld(Input.MousePosition);

                List<RaycastHit> hits = Engine.Instance.CollisionSystem.RaycastAll(Transform.Position, end, GameObject);

                foreach (var item in hits)
                {
                    Console.WriteLine(item.GameObject.Name);
                }
            }


        }

        /// <summary>
        /// Example of how to raycast with a specific range from a weapon
        /// </summary>
        private void Raycast_Weapon_Range()
        {
            int weaponRange = 10; //this would come form an equiped weapons.range property

            Vector2 mousePos = MainCamera.Instance.ScreenToWorld(Input.MousePosition);
            Vector2 direction = mousePos - Transform.Position;
            direction.Normalize();

            List<RaycastHit> hits = Engine.Instance.CollisionSystem.RaycastAll(Transform.Position, direction, weaponRange, boxCollider => boxCollider.GameObject != GameObject);
        
            foreach (var item in hits)
            {
                Console.WriteLine(item.GameObject.Name);
            }
        }




    }
}