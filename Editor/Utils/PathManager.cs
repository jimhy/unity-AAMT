using System.IO;

namespace AAMT.Editor.Components
{
    public static class PathManager
    {
        public const string PackagePath = "Packages/com.jimhy.aamt";
        public const string IconPath = PackagePath                + "/Editor/Images/Icons";
        public const string WindowsDir = PackagePath              + "/Editor/Windows";
        public const string ComponentDir = PackagePath            + "/Editor/Components";
        public const string MainPanelPath = WindowsDir            + "/MainWindow.uxml";
        public const string MenuTreeUssPath = ComponentDir        + "/MenuTree/MenuTree.uss";
        public const string PlatformSettingPanelPath = WindowsDir + "/PlatformSettingPanel/PlatformSettingPanel.uxml";

        public static string GetDirectoryName(string x) => x == null ? (string)null : Path.GetDirectoryName(x).Replace("\\", "/");
    }
}