using System;
using UnityEditor;
using UnityEngine;

namespace AAMT.Editor
{
    public static class Icons
    {
        /// <summary>
        /// 主页
        /// </summary>
        public static Icon HOME = new Icon("home");

        /// <summary>
        /// 下拉箭头
        /// </summary>
        public static Icon DROP_DOWN_ARROW = new Icon("dropDownArrow");

        /// <summary>
        /// 手机
        /// </summary>
        public static Icon PHONE = new Icon("phone");

        /// <summary>
        /// 文件夹
        /// </summary>
        public static Icon FOLDER = new Icon("folder");

        /// <summary>
        /// 设置
        /// </summary>
        public static Icon SETTING = new Icon("setting");
    }


    public class Icon
    {
        private string    _path;
        private Texture2D _texture2D;
        private Texture   _texture;

        public Icon(string path)
        {
            if (path.IndexOf("/", StringComparison.Ordinal) == -1)
                _path = $"{PathManager.IconPath}/{path}.png";
            else
                _path = path;
        }

        public Icon(Texture2D texture)
        {
            _texture2D = texture;
        }

        public Icon(Texture texture)
        {
            _texture = texture;
        }

        public Texture2D Texture2D
        {
            get
            {
                if (_texture2D != null) return _texture2D;
                if (!string.IsNullOrEmpty(_path)) return _texture2D = LoadTexture2D(_path);

                return null;
            }
        }

        public Texture Texture
        {
            get
            {
                if (_texture != null) return _texture;
                if (!string.IsNullOrEmpty(_path)) return _texture = LoadTexture(_path);

                return null;
            }
        }

        private Texture2D LoadTexture2D(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        private Texture LoadTexture(string path)
        {
            return AssetDatabase.LoadAssetAtPath<Texture>(path);
        }
    }
}