using UnityEngine.UIElements;

namespace AAMT.Editor.Components
{
    public interface IMenuItem
    {
        void ShowContentNode(VisualElement parent);
        void SetBackgroundColor(StyleColor styleColor);
    }

    public class ContentNode : BindableElement
    {
        public virtual void SetData(object data)
        {
        }
    }
}