using System.IO;

namespace AAMT.Editor
{
    public static class PathManager
    {
        public const string PackagePath = "Packages/com.jimhy.aamt";
        public const string IconPath = PackagePath                + "/Editor/Resource/Icons";
        public const string WindowsDir = PackagePath              + "/Editor/Scripts/Windows";
        public const string ComponentDir = PackagePath            + "/Editor/Scripts/Components";
        public const string MainPanelPath = WindowsDir            + "/MainWindow/MainWindow.uxml";
        public const string MenuTreeUssPath = ComponentDir        + "/MenuTree/MenuTree.uss";
        public const string PlatformSettingPanelPath = WindowsDir + "/PlatformSettingPanel/PlatformSettingPanel.uxml";
        public const string SettingPanelPath = WindowsDir + "/SettingPanel/SettingPanel.uxml";

        public static string GetDirectoryName(string x) => x == null ? (string)null : Path.GetDirectoryName(x).Replace("\\", "/");
    }
}