using UnityEditor;
using UnityEngine;

namespace AAMT.Editor.Components
{
    public static class Icons
    {
        /// <summary>
        /// 主页
        /// </summary>
        public static string HOME = "home";

        /// <summary>
        /// 下拉箭头
        /// </summary>
        public static string DROP_DOWN_ARROW = "dropDownArrow";

        /// <summary>
        /// 手机
        /// </summary>
        public static string PHONE = "phone";

        /// <summary>
        /// 设置
        /// </summary>
        public static string SETTING = "setting";


        public static Texture2D GetIconByName(string name)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>($"{PathManager.IconPath}/{name}.png");
        }

        public static Texture GetTextureByName(string name)
        {
            return AssetDatabase.LoadAssetAtPath<Texture>($"{PathManager.IconPath}/{name}.png");
        }
    }
}