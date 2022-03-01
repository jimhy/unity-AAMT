using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

namespace AAMT
{
    public static class Tools
    {
        internal static string FilterSpriteUri(string input)
        {
            return Regex.Replace(input, @"\?.+", "");
        }

        internal static void ParsingLoadUri(string input, out string abName, out string itemName,
            [CanBeNull] out string spriteName)
        {
            var bundleManager = AssetsManager.Instance.bundleManager;
            input = input.ToLower();
            var uri = input;
            abName = null;
            spriteName = null;
            itemName = null;
            var n = input.LastIndexOf("?", StringComparison.Ordinal);
            if (n != -1)
            {
                spriteName = input[(n + 1)..];
                uri = input[..n];
            }

            if (!bundleManager.PathToBundle.ContainsKey(uri))
            {
                Debug.LogErrorFormat("获取资源时，找不到对应的ab包。path:{0}", uri);
                return;
            }

            abName = bundleManager.PathToBundle[uri];
            n = uri.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = uri[(n + 1)..];
            }
        }
    }
}