using System.Text.RegularExpressions;
using Sirenix.OdinInspector;

namespace AAMT
{
    public class WindowDefine
    {
        public static string dataPath            = "Assets/AAMT/Data";
        public static string platformSettingPath = $"{dataPath}/Platforms";
        public static Regex  httpRegex           = new Regex(@"http(s)?://.+");

        public enum ABType
        {
            [LabelText("单个")]
            SINGLE,

            [LabelText("集合")]
            PACKAGE,
        }
    }
}