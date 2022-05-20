using System;
using UnityEditor;
using UnityEngine;

namespace AAMT
{
    [CustomEditor(typeof(SettingManager))]
    public class SettingManagerCustomEditor : Editor
    {
        private SerializedProperty _buildTarget;

        private void OnEnable()
        {
            _buildTarget = serializedObject.FindProperty("buildTarget");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            var buildTarget = (AssetSetting.BuildTarget) _buildTarget.enumValueIndex;
            if (buildTarget != AssetSetting.BuildTarget.editor)
            {
                if (GUILayout.Button("Build Bundles"))
                {
                    AssetsBundleBuilder.BuildAssetsBundles();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}