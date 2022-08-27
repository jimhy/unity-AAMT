using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace AAMT.Windos
{
    public class AssetBundleSettingWindow:OdinEditorWindow
    {
        [EnumToggleButtons]
        public ViewTool SomeField;

        protected override void OnEnable()
        {
            base.OnEnable();
        }
    }
}