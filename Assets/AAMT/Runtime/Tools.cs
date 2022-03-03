﻿using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AAMT
{
    public static class Tools
    {
        internal static string FilterSpriteUri(string input)
        {
            return Regex.Replace(input, @"\?.+", "");
        }

        internal static void ParsingLoadUri(string input, out string abName, out string itemName, out string spriteName)
        {
            ParsingLoadUri(input, out _, out abName, out itemName, out spriteName);
        }

        internal static void ParsingLoadUri(string input, out string uri, out string abName, out string itemName,
            out string spriteName)
        {
            if (AssetsManager.Instance.ResourceManager is LocalAssetManager)
            {
                ForEditor(input, out uri, out abName, out itemName, out spriteName);
            }
            else
            {
                ForBundle(input, out uri, out abName, out itemName, out spriteName);
            }
        }

        private static void ForBundle(string input, out string uri, out string abName, out string itemName,
            out string spriteName)
        {
            var bundleManager = AssetsManager.Instance.ResourceManager as BundleManager;
            input = input.ToLower();
            uri = input;
            abName = null;
            spriteName = null;
            itemName = null;
            if (bundleManager == null) return;
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

        private static void ForEditor(string input, out string uri, out string abName, out string itemName,
            out string spriteName)
        {
            input = input.ToLower();
            uri = input;
            abName = null;
            spriteName = null;
            itemName = null;

            var n = input.LastIndexOf("?", StringComparison.Ordinal);
            if (n != -1)
            {
                spriteName = input[(n + 1)..];
                uri = input[..n];
            }

            n = uri.LastIndexOf("/", StringComparison.Ordinal);
            if (n != -1)
            {
                itemName = uri[(n + 1)..];
            }
        }
    }
}