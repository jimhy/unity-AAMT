using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Object = System.Object;

namespace AAMT
{
    public class SpriteAtlasManagerEditor : SpriteAtlasManager
    {
        internal SpriteAtlasManagerEditor() : base(null)
        {
        }

        protected override IEnumerator StartGetAssetsAtlas<T>(string path, Action<T> callBack)
        {
            yield return 0;
#if UNITY_EDITOR

            Tools.ParsingLoadUri(path, out var uri, out var abName, out var atlasName, out string spriteName);
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(atlasName))
            {
                Debug.LogErrorFormat("加载资源失败,参数错误,abName:{0},atlasName:{1}", abName, atlasName);
                callBack?.Invoke(default);
                yield break;
            }

            AAMTSpriteAtlas atl;
            if (atlasMap.ContainsKey(atlasName))
            {
                atl = atlasMap[atlasName];
                callBack?.Invoke(atl as T);
                yield break;
            }

            atl = new AAMTSpriteAtlas();
            atlasMap[atlasName] = atl;
            uri = $"{SettingManager.AssetSetting.GetLoadPath}/{uri}";
            var sprites = AssetDatabase.LoadAllAssetsAtPath(uri);
            if (sprites == null)
            {
                callBack?.Invoke(default);
                yield break;
            }

            foreach (var sprite in sprites)
            {
                if (sprite is Sprite) atl.Add(sprite as Sprite);
            }

            callBack?.Invoke(atl as T);
#endif
        }

        protected override void StartGetAssetsSprite<T>(string path, Action<T> callBack)
        {
            Tools.ParsingLoadUri(path, out var uri, out var _, out var atlasName, out string spriteName);
            if (string.IsNullOrEmpty(atlasName) || string.IsNullOrEmpty(spriteName))
            {
                Debug.LogErrorFormat("加载资源失败,参数错误,abName:{0},spriteName:{1}", atlasName, spriteName);
                callBack?.Invoke(default);
                return;
            }

            AAMTRuntime.Instance.StartCoroutine(StartGetAssetsAtlas<AAMTSpriteAtlas>(path, (atl) =>
            {
                var result = atl.GetSprite(spriteName);

                if (result != null) callBack?.Invoke(result as T);
                else callBack?.Invoke(default);
            }));
        }
    }
}