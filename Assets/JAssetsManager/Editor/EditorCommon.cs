using System.Collections;
using System.Collections.Generic;
using JAssetsManager;
using UnityEditor;
using UnityEngine;

namespace JAssetsManager
{
    public class EditorCommon
    {
        public static void UpdateProgress(string title, int progress, int progressMax, string desc)
        {
            title = title + "...[" + progress + " - " + progressMax + "]";
            float value = (float) progress / (float) progressMax;
            EditorUtility.DisplayProgressBar(title, desc, value);
        }

    }
}