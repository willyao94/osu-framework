﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;

namespace osu.Framework.Input.Bindings
{
    /// <summary>
    /// Represent a combination of more than one <see cref="InputKey"/>s.
    /// </summary>
    public class KeyCombination : IEquatable<KeyCombination>
    {
        /// <summary>
        /// The keys.
        /// </summary>
        public readonly IEnumerable<InputKey> Keys;

        /// <summary>
        /// Construct a new instance.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public KeyCombination(IEnumerable<InputKey> keys)
        {
            Keys = keys.OrderBy(k => (int)k).ToArray();
        }

        /// <summary>
        /// Construct a new instance.
        /// </summary>
        /// <param name="keys">A comma-separated (KeyCode) string representation of the keys.</param>
        public KeyCombination(string keys)
            : this(keys.Split(',').Select(s => (InputKey)int.Parse(s)))
        {
        }

        /// <summary>
        /// Check whether the provided input is a valid pressedKeys for this combination.
        /// </summary>
        /// <param name="pressedKeys">The potential pressedKeys for this combination.</param>
        /// <returns>Whether the pressedKeys keys are valid.</returns>
        public bool IsPressed(KeyCombination pressedKeys) => !Keys.Except(pressedKeys.Keys).Any();

        public bool Equals(KeyCombination other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Keys.SequenceEqual(other.Keys);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyCombination)obj);
        }

        public override int GetHashCode() => Keys != null ? Keys.Select(b => b.GetHashCode()).Aggregate((h1, h2) => h1 * 17 + h2) : 0;

        public static implicit operator KeyCombination(InputKey singleButton) => new KeyCombination(new[] { singleButton });

        public static implicit operator KeyCombination(string stringRepresentation) => new KeyCombination(stringRepresentation);

        public static implicit operator KeyCombination(InputKey[] keys) => new KeyCombination(keys);

        public override string ToString() => Keys?.Select(b => ((int)b).ToString()).Aggregate((s1, s2) => $"{s1},{s2}") ?? string.Empty;

        public string ReadableString() => Keys?.Select(getReadableKey).Aggregate((s1, s2) => $"{s1}+{s2}") ?? string.Empty;

        private string getReadableKey(InputKey key)
        {
            switch (key)
            {
                case InputKey.Unknown:
                    return string.Empty;
                case InputKey.ShiftLeft:
                    return "LShift";
                case InputKey.ShiftRight:
                    return "RShift";
                case InputKey.ControlLeft:
                    return "LCtrl";
                case InputKey.ControlRight:
                    return "RCtrl";
                case InputKey.AltLeft:
                    return "LAlt";
                case InputKey.AltRight:
                    return "RAlt";
                case InputKey.WinLeft:
                    return "LWin";
                case InputKey.WinRight:
                    return "RWin";
                case InputKey.Escape:
                    return "Esc";
                case InputKey.BackSpace:
                    return "Backsp";
                case InputKey.Insert:
                    return "Ins";
                case InputKey.Delete:
                    return "Del";
                case InputKey.PageUp:
                    return "Pgup";
                case InputKey.PageDown:
                    return "Pgdn";
                case InputKey.CapsLock:
                    return "Caps";
                case InputKey.Number0:
                case InputKey.Keypad0:
                    return "0";
                case InputKey.Number1:
                case InputKey.Keypad1:
                    return "1";
                case InputKey.Number2:
                case InputKey.Keypad2:
                    return "2";
                case InputKey.Number3:
                case InputKey.Keypad3:
                    return "3";
                case InputKey.Number4:
                case InputKey.Keypad4:
                    return "4";
                case InputKey.Number5:
                case InputKey.Keypad5:
                    return "5";
                case InputKey.Number6:
                case InputKey.Keypad6:
                    return "6";
                case InputKey.Number7:
                case InputKey.Keypad7:
                    return "7";
                case InputKey.Number8:
                case InputKey.Keypad8:
                    return "8";
                case InputKey.Number9:
                case InputKey.Keypad9:
                    return "9";
                case InputKey.Tilde:
                    return "~";
                case InputKey.Minus:
                    return "-";
                case InputKey.Plus:
                    return "+";
                case InputKey.BracketLeft:
                    return "(";
                case InputKey.BracketRight:
                    return ")";
                case InputKey.Semicolon:
                    return ";";
                case InputKey.Quote:
                    return "\"";
                case InputKey.Comma:
                    return ",";
                case InputKey.Period:
                    return ".";
                case InputKey.Slash:
                    return "/";
                case InputKey.BackSlash:
                case InputKey.NonUSBackSlash:
                    return "\\";
                default:
                    return key.ToString();
            }
        }

        public static InputKey KeyToInputKey(Key key)
        {
            return (InputKey)key;
        }

        public static InputKey MouseButtonToInputKey(MouseButton button)
        {
            return (InputKey)((int)InputKey.FirstMouseButton + button);
        }

        public static KeyCombination FromInputState(InputState state)
        {
            List<InputKey> keys = new List<InputKey>();

            if (state.Mouse != null)
            {
                foreach (var button in state.Mouse.Buttons)
                    keys.Add(MouseButtonToInputKey(button));
            }

            if (state.Keyboard != null)
            {
                foreach (var key in state.Keyboard.Keys)
                    keys.Add(KeyToInputKey(key));
            }

            return new KeyCombination(keys);
        }
    }
}
