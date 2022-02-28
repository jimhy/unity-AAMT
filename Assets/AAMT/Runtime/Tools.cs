using System.Text.RegularExpressions;

namespace AAMT
{
    public class Tools
    {
        internal static string FilterSpriteUri(string input)
        {
            return Regex.Replace(input, @"\?.+", "");
        }
    }
}