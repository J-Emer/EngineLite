// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Input;

// namespace EngineLite.UI.Util
// {
//     public static class Input
//     {
//         private static KeyboardState currentKeyboard;
//         private static KeyboardState previousKeyboard;
//         private static MouseState currentMouse;
//         private static MouseState previousMouse;
//         public static Vector2 MousePosition => new Vector2(currentMouse.X, currentMouse.Y);
//         public static Vector2 MouseDelta => new Vector2(currentMouse.X - previousMouse.X, currentMouse.Y - previousMouse.Y);
//         // public static int ScrollDelta => currentMouse.ScrollWheelValue - previousMouse.ScrollWheelValue;
//         public static int ScrollDelta
//         {
//             get
//             {
//                 int delta = currentMouse.ScrollWheelValue - previousMouse.ScrollWheelValue;

//                 if (delta > 0)
//                     return 1;

//                 if (delta < 0)
//                     return -1;

//                 return 0;
//             }
//         }
//         public static bool ShiftDown()
//         {
//             return GetKey(Keys.LeftShift) || GetKey(Keys.Right);
//         }


//         public static void Update()
//         {
//             previousKeyboard = currentKeyboard;
//             previousMouse = currentMouse;

//             currentKeyboard = Keyboard.GetState();
//             currentMouse = Mouse.GetState();
//         }

//         // --- KEYBOARD ---

//         public static bool GetKey(Keys key)
//         {
//             return currentKeyboard.IsKeyDown(key);
//         }

//         public static bool GetKeyDown(Keys key)
//         {
//             return currentKeyboard.IsKeyDown(key) && previousKeyboard.IsKeyUp(key);
//         }

//         public static bool GetKeyUp(Keys key)
//         {
//             return currentKeyboard.IsKeyUp(key) && previousKeyboard.IsKeyDown(key);
//         }

//         public static Vector2 GetAxis()
//         {
//             float x = 0f;
//             float y = 0f;

//             // Horizontal
//             if (GetKey(Keys.A) || GetKey(Keys.Left)) x -= 1f;
//             if (GetKey(Keys.D) || GetKey(Keys.Right)) x += 1f;

//             // Vertical
//             if (GetKey(Keys.W) || GetKey(Keys.Up)) y -= 1f;
//             if (GetKey(Keys.S) || GetKey(Keys.Down)) y += 1f;

//             Vector2 axis = new Vector2(x, y);

//             if (axis != Vector2.Zero)
//                 axis.Normalize(); // prevents faster diagonal movement

//             return axis;
//         }

//         // --- MOUSE BUTTONS ---

//         public static bool GetMouse(int button)
//         {
//             return GetMouseState(button, currentMouse);
//         }

//         public static bool GetMouseDown(int button)
//         {
//             return GetMouseState(button, currentMouse) && !GetMouseState(button, previousMouse);
//         }

//         public static bool GetMouseUp(int button)
//         {
//             return !GetMouseState(button, currentMouse) && GetMouseState(button, previousMouse);
//         }

//         private static bool GetMouseState(int button, MouseState state)
//         {
//             return button switch
//             {
//                 0 => state.LeftButton == ButtonState.Pressed,
//                 1 => state.RightButton == ButtonState.Pressed,
//                 2 => state.MiddleButton == ButtonState.Pressed,
//                 _ => false
//             };
//         }
//     }
// }