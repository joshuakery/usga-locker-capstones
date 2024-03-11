using System.Linq;
using TMPro;

namespace JoshKery.GenericUI.Text
{
    public static class AddNoBreakTags
    {
        public static void AddNoBreakTagsToText(TMP_Text tmp_text)
        {
            if (tmp_text != null)
            {
                string[] words = tmp_text.text.Split(" ");

                tmp_text.text = System.String.Join(" ", words.Take(words.Length - 3).ToArray()) +
                    " <nobr>" +
                    words[words.Length - 2] + " " + words[words.Length - 1] +
                    "</nobr>";  
            }    
        }
    }
}


