using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ShowEditorIcon : EditorWindow
{
    private VisualElement root;

    [MenuItem("AAMT/ShowEditorIcon")]
    public static void ShowExample()
    {
        ShowEditorIcon wnd = GetWindow<ShowEditorIcon>();
        wnd.titleContent = new GUIContent("ShowEditorIcon");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.jimhy.aamt/Editor/ShowEditorIcons/ShowEditorIcon.uxml");
        visualTree.CloneTree(root);

        GetIcons();
    }

    private void GetIcons()
    {
        var iconsContainer = root.Q<GroupBox>("mGroupBox");
        iconsContainer.Clear();
        var textures = Resources.FindObjectsOfTypeAll<Texture2D>();
        iconsContainer.RegisterCallback<ClickEvent>(OnClickDown);
        foreach (var texture2D in textures)
        {
            // var img = EditorGUIUtility.FindTexture(texture2D.name);
            if (texture2D == null) continue;

            var item = new VisualElement();
            item.name = "Item";
            iconsContainer.Add(item);
            item.userData = texture2D.name;

            var icon = new VisualElement();
            icon.name = "Icon";
            item.Add(icon);

            var nameLabel = new Label();
            nameLabel.name = "Name";
            item.Add(nameLabel);
            if (texture2D.name.Length >= 6)
                nameLabel.text = texture2D.name.Substring(0, 6) + "...";
            else
                nameLabel.text = texture2D.name;

            icon.style.backgroundImage = new StyleBackground(texture2D);
        }
    }

    private void OnClickDown<TEventType>(TEventType evt) where TEventType : EventBase<TEventType>, new()
    {
        var target                        = evt.target as VisualElement;
        if (target.name != "Item") target = target.parent;
        Debug.Log(target.userData);
    }
}