using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace AAMT.Editor
{
    public static class MiscUtils
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

        public static List<string> EnumToStringList(Type t)
        {
            var list   = new List<string>();
            var fields = t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            foreach (var info in fields)
            {
                list.Add(info.Name);
            }

            return list;
        }

        public static int StringToEnum<T>(string enumString)
        {
            var t      = typeof(T);
            var fields = t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
            for (var i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == enumString) return i;
            }

            return -1;
        }
    }
}