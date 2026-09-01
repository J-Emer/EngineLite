using System.Collections.Generic;
using EngineLite.Core;
using EngineLite.UI.Controls;
using Microsoft.Xna.Framework;


namespace EngineLite.UI.Util

{
    public class MouseInteractions
    {
        private Control hoveredControl;
        private Control pressedControl;
        private Control capturedControl;
        public Control FocusedControl { get; private set; }
        private KeyBoardInteraction keyBoardInteraction;

        public MouseInteractions()
        {
            keyBoardInteraction = new KeyBoardInteraction(this);
        }

        public void HandleInteraction(List<Control> controls, List<ContainerControl> overlays)
        {
            Control newHovered = hoveredControl;

            //-----------------------------------
            // Find hovered control
            //-----------------------------------

            if (capturedControl == null)
            {
                newHovered = null;


                //-----------------------------------
                // Overlays get first priority
                //-----------------------------------

                Control overlayHit = OverlayHitTest(overlays);

                if (overlayHit != null)
                {
                    newHovered = overlayHit;
                }
                else
                {
                    //-----------------------------------
                    // Normal controls
                    //-----------------------------------

                    for (int i = controls.Count - 1; i >= 0; i--)
                    {
                        var hit = controls[i].MouseHitTest();

                        if (hit != null)
                        {
                            newHovered = hit;
                            break;
                        }
                    }
                }


                //-----------------------------------
                // Normal controls
                //-----------------------------------

                for (int i = controls.Count - 1; i >= 0; i--)
                {
                    var hit = controls[i].MouseHitTest();

                    if (hit != null)
                    {
                        newHovered = hit;
                        break;
                    }
                }




            }

            //-----------------------------------
            // Scroll Wheel
            //-----------------------------------

            if (Input.ScrollDelta != 0)
            {
                hoveredControl?.Scroll(CreateMouseEvent(-1));
            }

            //-----------------------------------
            // Hover Enter / Exit
            //-----------------------------------

            if (newHovered != hoveredControl)
            {
                hoveredControl?.MouseExit(CreateMouseEvent(-1));

                hoveredControl = newHovered;

                hoveredControl?.MouseEnter(CreateMouseEvent(-1));
            }

            hoveredControl?.MouseHover(CreateMouseEvent(-1));

            //-----------------------------------
            // Mouse Move
            //-----------------------------------

            bool mouseMoved = Input.MouseDelta != Vector2.Zero;

            if (mouseMoved)
            {
                // Send move to captured control first
                if (capturedControl != null)
                {
                    capturedControl.MouseMove(CreateMouseEvent(-1));
                }
                else
                {
                    hoveredControl?.MouseMove(CreateMouseEvent(-1));
                }
            }

            //-----------------------------------
            // Mouse Buttons
            //-----------------------------------

            for (int button = 0; button < 5; button++)
            {
                //-----------------------------------
                // Mouse Down
                //-----------------------------------

                if (Input.GetMouseDown(button))
                {
                    pressedControl = hoveredControl;

                    SetFocus(pressedControl);

                    capturedControl = pressedControl;

                    pressedControl?.MouseDown(CreateMouseEvent(button));
                }

                //-----------------------------------
                // Mouse Up
                //-----------------------------------

                if (Input.GetMouseUp(button))
                {
                    pressedControl?.MouseUp(CreateMouseEvent(button));

                    pressedControl = null;

                    // NEW
                    capturedControl = null;
                }
            }

            keyBoardInteraction.Update();
        }

        private UIMouseEvent CreateMouseEvent(int button)
        {
            return new UIMouseEvent(
                button,
                Input.MousePosition,
                Input.MouseDelta,
                Input.ScrollDelta);
        }

        private void SetFocus(Control control)
        {
            //-----------------------------------
            // Already Focused
            //-----------------------------------

            if (FocusedControl == control)
            {
                return;
            }

            //-----------------------------------
            // Clear Previous
            //-----------------------------------

            if (FocusedControl != null)
            {
                FocusedControl.HasFocus = false;
            }

            //-----------------------------------
            // Set New
            //-----------------------------------

            FocusedControl = control;

            //-----------------------------------
            // Apply Focus
            //-----------------------------------

            if (FocusedControl != null)
            {
                FocusedControl.HasFocus = true;
            }
        }



        private Control OverlayHitTest(List<ContainerControl> _overlayControls)
        {
            for (int i = _overlayControls.Count - 1; i >= 0; i--)
            {
                var hit = _overlayControls[i].MouseHitTest();

                if (hit != null)
                {
                    return hit;
                }
            }

            return null;
        }


    }
}