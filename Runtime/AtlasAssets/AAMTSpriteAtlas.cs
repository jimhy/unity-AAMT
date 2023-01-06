using System.Collections.Generic;
using UnityEngine;

namespace AAMT
{
    public class AAMTSpriteAtlas : Object
    {
        private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private string _atlasName = "";

        public AAMTSpriteAtlas(string atlasName)
        {
            _atlasName = atlasName;
        }
        
        public virtual Sprite GetSprite(string spriteName)
        {
            spriteName = spriteName.ToLower();
            if (_sprites.ContainsKey(spriteName)) return _sprites[spriteName];
            Debug.LogError($"Can not found spriteName:{spriteName} frome atlas:{_atlasName}");
            return null;
        }

        public bool HasSprite(string spriteName)
        {
            spriteName = spriteName.ToLower();
            return _sprites.ContainsKey(spriteName);
        }

        public Sprite[] GetSprites()
        {
            var sprites = new Sprite[_sprites.Count];
            var i       = 0;
            foreach (var key in _sprites)
            {
                sprites[i++] = key.Value;
            }

            return sprites;
        }

        internal void Add(AssetBundleRequest request)
        {
            foreach (var o in request.allAssets)
            {
                var sprite                                          = o as Sprite;
                if (sprite != null) _sprites[sprite.name.ToLower()] = sprite;
            }
        }

        internal void Add(Sprite sprite)
        {
            if (sprite != null) _sprites[sprite.name.ToLower()] = sprite;
        }
    }
}