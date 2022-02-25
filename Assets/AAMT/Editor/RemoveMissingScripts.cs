using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class RemoveMissingScripts
{
    [MenuItem("Assets/AAMT/Remove Miss Script", priority = -100)]
    private static void FindInSelected()
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

            var fs = Directory.GetFiles(p, "*", SearchOption.AllDirectories);
            files.AddRange(fs);
        }

        foreach (var file in files)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(file);
            if (go != null)
            {
                RemoveMissingScript(go);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void RemoveMissingScript(GameObject go)
    {
        var i = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        if (i > 0)
        {
            Debug.LogFormat("删除了丢失的脚本:{0}", go.name);
        }

        var trs = go.transform.GetComponentsInChildren<Transform>();
        foreach (var tr in trs)
        {
            i = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(tr.gameObject);
            if (i > 0)
            {
                Debug.LogFormat("删除了丢失的脚本:{0}", tr.gameObject.name);
            }
        }
    }
}