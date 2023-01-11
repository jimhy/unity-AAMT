using System.Globalization;
using UnityEngine;

namespace AAMT.Editor.Components
{
    public static class ColorUtils
    {
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", string.Empty);
            hex = hex.Replace("#", string.Empty);
            byte a = byte.MaxValue;
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }
    }
}