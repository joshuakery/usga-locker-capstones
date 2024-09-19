using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

namespace JoshKery.GenericUI.Text
{
    public static class RemoveDoubleCarriageReturns
    {
        static string pattern = "(\\n){2,}";
        static string replacePattern = "\n";

        public static void Process(TMP_Text tmp_text)
        {
            if (tmp_text != null)
            {
                tmp_text.text = Regex.Replace(tmp_text.text, pattern, replacePattern, RegexOptions.IgnoreCase);
            }
        }
    }
}


