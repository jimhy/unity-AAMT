using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

namespace AAMT.Editor
{
    public class WindowDefine
    {
        public static string dataPath = "Assets/AAMT/Data";
        public static string platformSettingPath = $"{dataPath}/Platforms";
        public static Regex httpRegex = new Regex(@"http(s)?://.+");

        public enum ABType
        {
            [LabelText("集合")]
            PACKAGE,


            [LabelText("单个")]
            SINGLE,

            [LabelText("父级")]
            PARENT,
        }
    }
}