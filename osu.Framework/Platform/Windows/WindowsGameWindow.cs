﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Runtime.InteropServices;
using osu.Framework.Platform.Windows.Native;

namespace osu.Framework.Platform.Windows
{
    internal class WindowsGameWindow : DesktopGameWindow
    {
        private const int seticon_message = 0x0080;

        private IconGroup iconGroup;
        private Icon smallIcon;
        private Icon largeIcon;

        public override void SetIconFromStream(Stream stream)
        {
            if (WindowInfo.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Window must be created before an icon can be set.");

            iconGroup = new IconGroup(stream);

            smallIcon = iconGroup.CreateIcon(24, 24);
            largeIcon = iconGroup.CreateIcon(256, 256);

            SendMessage(WindowInfo.Handle, seticon_message, (IntPtr)0, smallIcon.Handle);
            SendMessage(WindowInfo.Handle, seticon_message, (IntPtr)1, largeIcon.Handle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
