using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cyberpunch_game
{
    class InputManager : GameComponent
    {
        #region Variables
        public IControllable controllable; // Reference to the object controlled by this input class
        PlayerIndex controllerIndex;

        // Keyboard
        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;
        Dictionary<Buttons, Keys> keyboardMap; // Contains translations between gamepad and keyboard

        // GamePad
        GamePadState gamePadState;
        GamePadState lastGamePadState;

        public List<KeyActionRelation> ActionList;

        #endregion

        #region Constructors
        public InputManager(Game Game, IControllable Controllable, List<KeyActionRelation> Actions) // Used if you don't care about using a keyboard
            : this(Game, Controllable, Actions, null) { }

        public InputManager(Game Game, IControllable Controllable, List<KeyActionRelation> Actions, Dictionary<Buttons, Keys> keyboardMap)
            : base(Game)
        {
            this.controllable = Controllable;
            this.controllerIndex = Controllable.PlayerIndex;
            this.keyboardMap = keyboardMap;
            this.ActionList = Actions;
        }
        #endregion
        public override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState(controllerIndex).GetPressedKeys().Length > 0 //Lets not check for input if no keys are being pressed.
            || (GamePad.GetState(controllerIndex).IsConnected && keyboardMap == null)) // And if we have no keyboard, lets not check for keys if the gamepad isnt connected.
            {
                foreach (KeyActionRelation keyRelation in ActionList) // Loops through all the keyrelations in the Actions list
                {
                    if (keyRelation.Pressed) // If the keyrelation should be checked for a press
                    {
                        if (IsKeyPressed(keyRelation.Button)) // Check if the key is pressed
                            controllable.SetInput(keyRelation.Action); // If its pressed, send the action to the controllable unit.
                    }
                    else if (keyRelation.Down) // If the keyrelation should be checked for holding a key
                    {
                        if (IsKeyDown(keyRelation.Button)) // Check if the key is down
                            controllable.SetInput(keyRelation.Action);// If its down, send the action to the controllable unit.
                    }
                }
            }

            // Retrieve states
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState(controllerIndex);
            lastGamePadState = gamePadState;
            gamePadState = GamePad.GetState(controllerIndex);

            base.Update(gameTime);
        }
        #region Detect Keys/Sticks
        public bool IsKeyDown(Buttons button) // Detects a key/button being held down
        {
            bool pressed = false;
            if (gamePadState.IsConnected) // Cant have buttons pressed if the controller isn't connected
                pressed = gamePadState.IsButtonDown(button);
            else if (keyboardMap != null) // If we have a keyboard or not
                pressed = keyboardState.IsKeyDown(keyboardMap[button]); // keyboardMap[button] will return the corresponding key from the map
            return pressed;
        }
        public bool IsKeyPressed(Buttons button) // Detects a single press of the key/button
        {
            bool pressed = false;
            if (gamePadState.IsConnected) // Cant have buttons pressed if the controller isn't connected
                pressed = (gamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button));
            else if (keyboardMap != null) // If we have a keyboard or not
                pressed = (keyboardState.IsKeyDown(keyboardMap[button]) && lastKeyboardState.IsKeyUp(keyboardMap[button])); // keyboardMap[button] will return the corresponding key from the map
            return pressed;
        }
        public Vector2 GetLeftThumbStick()
        {
            Vector2 thumbPosition = Vector2.Zero;
            if (gamePadState.IsConnected)
                thumbPosition = gamePadState.ThumbSticks.Left;
            else if (keyboardMap != null)
            {
                // Creates a vector based on the corresponding keys from the keyboard
                if (keyboardState.IsKeyDown(keyboardMap[Buttons.LeftThumbstickUp]))
                    thumbPosition.Y = 1;
                else if (keyboardState.IsKeyDown(keyboardMap[Buttons.LeftThumbstickDown]))
                    thumbPosition.Y = -1;
                if (keyboardState.IsKeyDown(keyboardMap[Buttons.LeftThumbstickRight]))
                    thumbPosition.X = 1;
                else if (keyboardState.IsKeyDown(keyboardMap[Buttons.LeftThumbstickLeft]))
                    thumbPosition.X = -1;
            }
            return thumbPosition;
        }
        public Vector2 GetRightThumbStick()
        {
            Vector2 thumbPosition = Vector2.Zero;
            if (gamePadState.IsConnected)
                thumbPosition = gamePadState.ThumbSticks.Right;
            else if (keyboardMap != null)
            {
                // Creates a vector based on the corresponding keys from the keyboard
                if (keyboardState.IsKeyDown(keyboardMap[Buttons.RightThumbstickUp]))
                    thumbPosition.Y = 1;
                else if (keyboardState.IsKeyDown(keyboardMap[Buttons.RightThumbstickDown]))
                    thumbPosition.Y = -1;
                if (keyboardState.IsKeyDown(keyboardMap[Buttons.RightThumbstickRight]))
                    thumbPosition.X = 1;
                else if (keyboardState.IsKeyDown(keyboardMap[Buttons.RightThumbstickLeft]))
                    thumbPosition.X = -1;
            }
            return thumbPosition;
        }
        #endregion
    }
}
