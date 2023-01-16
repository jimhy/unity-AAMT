using System.Reflection;

namespace AAMT.Editor
{
    public static class FileSaveUtils
    {
        public static void SetDataProperty(object data, string propertyName, object val)
        {
            var t     = data.GetType();
            var field = t.GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field != null) field.SetValue(data, val);
        }
    }
}