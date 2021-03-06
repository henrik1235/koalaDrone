﻿using System;

namespace DroneLibrary
{
    public class BinaryHelper
    {
        public static ushort ReverseBytes(ushort value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;
            return (ushort)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static uint ReverseBytes(uint value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;
            return (uint)((value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                         (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24);
        }

        public static ulong ReverseBytes(ulong value)
        {
            if (!BitConverter.IsLittleEndian)
                return value;

            return (value & (ulong)0x00000000000000FFUL) << 56 | (value & (ulong)0x000000000000FF00UL) << 40 |
                   (value & (ulong)0x0000000000FF0000UL) << 24 | (value & (ulong)0x00000000FF000000UL) << 8 |
                   (value & (ulong)0x000000FF00000000UL) >> 8 | (value & (ulong)0x0000FF0000000000UL) >> 24 |
                   (value & (ulong)0x00FF000000000000UL) >> 40 | (value & (ulong)0xFF00000000000000UL) >> 56;
        }
    }
}