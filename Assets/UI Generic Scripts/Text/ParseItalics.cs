using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

namespace JoshKery.GenericUI.Text
{
    public static class ParseItalics
    {
        private static Regex _replaceItalics = new(@"\*([^\*]*)\*");

        public static void ParseItalicsInText(TMP_Text tmp_text)
        {
            if (tmp_text != null)
            {
                tmp_text.text = _replaceItalics.Replace(tmp_text.text, @"<i>$1</i>");
            }
        }
    }
}


