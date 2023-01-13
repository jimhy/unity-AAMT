namespace AAMT
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class LabelTextAttribute : Attribute
    {
        public string Text;

        public bool NicifyText;

        public LabelTextAttribute(string text) => this.Text = text;
    }
}