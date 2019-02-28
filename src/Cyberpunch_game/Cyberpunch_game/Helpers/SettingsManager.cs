using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Cyberpunch_game
{
    #region Settings Structs
    public struct KeyboardSettings
    {
        public Keys A;
        public Keys B;
        public Keys X;
        public Keys Y;
        public Keys LeftShoulder;
        public Keys RightShoulder;
        public Keys LeftTrigger;
        public Keys RightTrigger;
        public Keys LeftStick;
        public Keys RightStick;
        public Keys Back;
        public Keys Start;
        public Keys DPadDown;
        public Keys DPadLeft;
        public Keys DPadRight;
        public Keys DPadUp;
        public Keys LeftThumbstickDown;
        public Keys LeftThumbstickLeft;
        public Keys LeftThumbstickRight;
        public Keys LeftThumbstickUp;
        public Keys RightThumbstickDown;
        public Keys RightThumbstickLeft;
        public Keys RightThumbstickRight;
        public Keys RightThumbstickUp;
    } // Settings between Keyboard and GamePad
    public struct KeyActionRelation
    {
        public GameAction Action;   // Contains the properties of an action.
        public Buttons Button;      // The GamePadButton to perform this action.
        public bool Down;           // Boolean saying if this is a key to be checked if being held down. Can't have same value as Down.
        public bool Pressed;        // Boolean saying if this key is a key to be checked if being pressed once. Can't have same value as Down. 
    }
    public struct GameSettings// Game Settings
    {
        public bool PreferredFullScreen;
        public int PreferredWindowWidth;
        public int PreferredWindowHeight;
        public KeyboardSettings KeyboardSettings;
        //public KeyRelation[] KeyRelations;
        public Vector2 Direction;
    }
    #endregion

    public class SettingsManager
    {
        #region Create Files
        public static void InitializeFiles()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.FileExists("settings.xml"))
                {
                    isf.CreateFile("settings.xml").Dispose();
                    XDocument xml;
                    using (Stream stream = File.Open("Content/settings.xml", FileMode.Open, FileAccess.Read))
                    {
                        xml = XDocument.Load(stream);
                    }
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("settings.xml", FileMode.Create, isf))
                    {
                        xml.Save(stream);
                    }
                }
            }
        }
        #endregion
        #region Save and Load functions
        public static GameSettings Load(string settingsFilename)
        {
            GameSettings settings; // Creates a new instance of GameSettings
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings)); // Serializer of type GameSettings
#if WINDOWS
            using (Stream stream = File.OpenRead("Content/"+settingsFilename))// Open and Read-stream from the filename entered
            {
                settings = (GameSettings)serializer.Deserialize(stream); //Deserialize the xml file stream and fill the data into a GameSettings object
            }
#elif XBOX
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(settingsFilename))
                {
                    isf.GetDirectoryNames("*");
                }
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(settingsFilename, FileMode.Open, FileAccess.Read, isf))
                {
                    settings = (GameSettings)serializer.Deserialize(stream);
                }
            }
#endif
            return settings;
        }
        public static void Save(string settingsFileName, GameSettings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings)); // Serializer of type GameSettings
    #if WINDOWS
            using (Stream stream = File.OpenWrite(settingsFileName))// Open and Write-stream from the filname entered
            {
                serializer.Serialize(stream, settings); // serialize the GameSettings data into the stream
            }
    #elif XBOX
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(settingsFileName, FileMode.OpenOrCreate, isf))
                {
                    serializer.Serialize(stream, settings);
                }
            }
    #endif
        }
        #endregion
        public static Dictionary<Buttons, Keys> GetKeyboardDictionary(KeyboardSettings keyboard)
        {
            Dictionary<Buttons, Keys> dictionary = new Dictionary<Buttons, Keys>(); // Dictionary containing bindings between gamepad and keyboard
            dictionary.Add(Buttons.A, keyboard.A);
            dictionary.Add(Buttons.B, keyboard.B);
            dictionary.Add(Buttons.X, keyboard.X);
            dictionary.Add(Buttons.Y, keyboard.Y);
            dictionary.Add(Buttons.LeftShoulder, keyboard.LeftShoulder);
            dictionary.Add(Buttons.RightShoulder, keyboard.RightShoulder);
            dictionary.Add(Buttons.LeftTrigger, keyboard.LeftTrigger);
            dictionary.Add(Buttons.RightTrigger, keyboard.RightTrigger);
            dictionary.Add(Buttons.LeftStick, keyboard.LeftStick);
            dictionary.Add(Buttons.RightStick, keyboard.RightStick);
            dictionary.Add(Buttons.Back, keyboard.Back);
            dictionary.Add(Buttons.Start, keyboard.Start);
            dictionary.Add(Buttons.DPadDown, keyboard.DPadDown);
            dictionary.Add(Buttons.DPadLeft, keyboard.DPadLeft);
            dictionary.Add(Buttons.DPadRight, keyboard.DPadRight);
            dictionary.Add(Buttons.DPadUp, keyboard.DPadUp);
            dictionary.Add(Buttons.LeftThumbstickDown, keyboard.LeftThumbstickDown);
            dictionary.Add(Buttons.LeftThumbstickLeft, keyboard.LeftThumbstickLeft);
            dictionary.Add(Buttons.LeftThumbstickRight, keyboard.LeftThumbstickRight);
            dictionary.Add(Buttons.LeftThumbstickUp, keyboard.LeftThumbstickUp);
            dictionary.Add(Buttons.RightThumbstickDown, keyboard.RightThumbstickDown);
            dictionary.Add(Buttons.RightThumbstickLeft, keyboard.RightThumbstickLeft);
            dictionary.Add(Buttons.RightThumbstickRight, keyboard.RightThumbstickRight);
            dictionary.Add(Buttons.RightThumbstickUp, keyboard.RightThumbstickUp);
            return dictionary;
        }

    }
}
