using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimationClipCompression
{
    [MenuItem("Tools/压缩AnimationClip", false, 11)]
    private static void OnPostprocessModel()
    {
        var files = new List<String>();
        foreach (var item in Selection.objects)
        {
            var p = AssetDatabase.GetAssetPath(item);
            var f = Path.GetFileName(p);
            if (f.LastIndexOf(".", StringComparison.Ordinal) != -1)
            {
                files.Add(AssetDatabase.GetAssetPath(item.GetInstanceID()));
                continue;
            }

            var fs = Directory.GetFiles(p, "*.anim", SearchOption.AllDirectories);
            files.AddRange(fs);
        }

        foreach (var file in files)
        {
            var ani = AssetDatabase.LoadAssetAtPath<AnimationClip>(file);
            if (ani != null) Start(ani);
        }
    }

    private static void Start(AnimationClip theAnimation)
    {
        try
        {
            //去除scale曲线
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
            {
                string name = theCurveBinding.propertyName.ToLower();
                if (name.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                }
            }

            Debug.LogFormat("压缩动作文件:{0}", theAnimation.name);
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! animationPath : {0} error: {1}",
                "assetPath", e));
        }
    }
}