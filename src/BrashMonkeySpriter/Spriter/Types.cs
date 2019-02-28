﻿/*==========================================================================
 * Project: BrashMonkeySpriter
 * File: Types.cs
 *
 *==========================================================================
 * Author:
 * Geoff "NowSayPillow" Lodder
 *==========================================================================*/

namespace BrashMonkeySpriter.Spriter {
    public enum SpinDirection : int {
        None = 0,
        Clockwise = -1,
        CounterClockwise = 1
    }

    public enum TimelineType : int {
        Body = 0,
        Bone = 1
    }
}
