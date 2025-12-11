using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdiumPhoton.Extra
{
    class Helpers
    {
        public string StripColorTags(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            return System.Text.RegularExpressions.Regex.Replace(text, "</?color[^>]*>", "");
        }
    }
}
