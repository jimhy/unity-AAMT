using System.Text.RegularExpressions;

namespace AAMT.Editor
{
    public class WindowDefine
    {
        public static string dataPath = "Assets/AAMT/Data";
        public static string platformSettingPath = $"{dataPath}/Platforms";
        public static Regex httpRegex = new Regex(@"http(s)?://.+");

        public enum ABType
        {
            PACKAGE,
            SINGLE,
            PARENT,
        }
    }
}