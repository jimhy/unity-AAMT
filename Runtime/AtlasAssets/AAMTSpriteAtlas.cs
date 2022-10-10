using System.Collections.Generic;
using UnityEngine;

namespace AAMT
{
    public class AAMTSpriteAtlas : Object
    {
        private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

        public virtual Sprite GetSprite(string spriteName)
        {
            if (_sprites.ContainsKey(spriteName)) return _sprites[spriteName];
            return null;
        }

        public bool HasSprite(string spriteName)
        {
            return _sprites.ContainsKey(spriteName);
        }

        public Sprite[] GetSprites()
        {
            var sprites = new Sprite[_sprites.Count];
            var i = 0;
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
                var sprite = o as Sprite;
                if (sprite != null) _sprites[sprite.name] = sprite;
            }
        }

        internal void Add(Sprite sprite)
        {
            if (sprite != null) _sprites[sprite.name] = sprite;
        }
    }
}