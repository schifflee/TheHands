﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Qellatalo.Nin.TheHands.KeyboardHandler;
using static Qellatalo.Nin.TheHands.WIHandler;

namespace Qellatalo.Nin.TheHands
{
    /// <summary>
    /// Make windows keyboard actions.
    /// </summary>
    [Obsolete("Static use will be removed in future releases. Please instanciate KeyboardHandler instead.")]
    public static class Keyboard
    {
        /// <summary>
        /// Default delay (milliseconds) after a keyboard action.
        /// </summary>
        public static int DefaultKeyboardActionDelay { get; set; } = 0;

        /// <remarks>
        /// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
        /// 
        /// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
        /// </remarks>
        internal static bool IsExtendedKey(Keys keyCode)
        {
            if (keyCode == Keys.Menu ||
                keyCode == Keys.LMenu ||
                keyCode == Keys.RMenu ||
                keyCode == Keys.Control ||
                keyCode == Keys.RControlKey ||
                keyCode == Keys.Insert ||
                keyCode == Keys.Delete ||
                keyCode == Keys.Home ||
                keyCode == Keys.End ||
                keyCode == Keys.Prior ||
                keyCode == Keys.Next ||
                keyCode == Keys.Right ||
                keyCode == Keys.Up ||
                keyCode == Keys.Left ||
                keyCode == Keys.Down ||
                keyCode == Keys.NumLock ||
                keyCode == Keys.Cancel ||
                keyCode == Keys.Snapshot ||
                keyCode == Keys.Divide)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// According to http://inputsimulator.codeplex.com, this need to be checked for character input
        /// </summary>
        /// <param name="wScan"></param>
        /// <returns></returns>
        internal static bool IsExtendedKey(ushort wScan)
        {
            return (wScan & 0xFF00) == 0xE000;
        }

        /// <summary>
        /// Releases a specified key.
        /// </summary>
        /// <param name="keyCode">The key to be released.</param>
        public static void KeyUp(Keys keyCode)
        {
            INPUT input = new INPUT
            {
                type = SendInputEventType.InputKeyboard,
                mkhi =
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = (ushort) keyCode,
                        //wScan = 0,
                        dwFlags = (IsExtendedKey(keyCode) ?
                            KeyboardEventFlags.KEYEVENTF_KEYUP | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY :
                            KeyboardEventFlags.KEYEVENTF_KEYUP)
                        //time = 0,
                        //dwExtraInfo = IntPtr.Zero
                    }
                }
            };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }
        /// <summary>
        /// Releases a specified key.
        /// </summary>
        /// <param name="character">The key to be released.</param>
        private static void characterUp(char character)
        {
            INPUT input = new INPUT
            {
                type = SendInputEventType.InputKeyboard,
                mkhi =
                {
                    ki = new KEYBDINPUT
                    {
                        wScan = character,
                        dwFlags = (IsExtendedKey(character) ?
                            KeyboardEventFlags.KEYEVENTF_KEYUP | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY :
                            KeyboardEventFlags.KEYEVENTF_KEYUP)
                    }
                }
            };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Presses a specified key.
        /// </summary>
        /// <param name="keyCode">The key to be pressed.</param>
        public static void KeyDown(Keys keyCode)
        {
            INPUT input =
                new INPUT
                {
                    type = SendInputEventType.InputKeyboard,
                    mkhi =
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = (ushort) keyCode,
                            //wScan = 0,
                            dwFlags = IsExtendedKey(keyCode) ? KeyboardEventFlags.KEYEVENTF_UNICODE | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY : KeyboardEventFlags.KEYEVENTF_UNICODE
                            //time = 0,
                            //dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Presses a specified key.
        /// </summary>
        /// <param name="character">The key to be pressed.</param>
        private static void characterDown(char character)
        {
            INPUT input =
                new INPUT
                {
                    type = SendInputEventType.InputKeyboard,
                    mkhi =
                    {
                        ki = new KEYBDINPUT
                        {
                            wScan = character,
                            dwFlags = IsExtendedKey(character) ? KeyboardEventFlags.KEYEVENTF_UNICODE | KeyboardEventFlags.KEYEVENTF_EXTENDEDKEY : KeyboardEventFlags.KEYEVENTF_UNICODE
                        }
                    }
                };
            SendInput(1, ref input, INPUT_SIZE);
            System.Threading.Thread.Sleep(DefaultKeyboardActionDelay);
        }

        /// <summary>
        /// Types a specified key.
        /// </summary>
        /// <param name="key">The key to be typed.</param>
        public static void KeyTyping(Keys key)
        {
            KeyDown(key);
            KeyUp(key);
        }

        /// <summary>
        /// Types a specified key.
        /// </summary>
        /// <param name="character">The key to be typed.</param>
        public static void CharacterInput(char character)
        {
            characterDown(character);
            characterUp(character);
        }
        /// <summary>
        /// Types a text.
        /// </summary>
        /// <param name="text">The text to be typed.</param>
        public static void StringInput(string text)
        {
            foreach(char c in text)
            {
                CharacterInput(c);
            }
        }
    }
}
