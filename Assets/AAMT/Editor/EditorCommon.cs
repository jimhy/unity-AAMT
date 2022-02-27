using UnityEditor;

namespace AAMT
{
    public class EditorCommon
    {
        internal static void UpdateProgress(string title, int progress, int progressMax, string desc)
        {
            title = title + "...[" + progress + " - " + progressMax + "]";
            float value = (float) progress / (float) progressMax;
            EditorUtility.DisplayProgressBar(title, desc, value);
        }

        internal static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }
        
        internal static void ClearConsole()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}