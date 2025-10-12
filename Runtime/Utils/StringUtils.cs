using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QbGameLib_Utils.Utils
{
    public static class StringUtils
    {
        public static string[] Regex(this string value, string pattern)
        {
            Regex rg = new Regex(pattern); 
            MatchCollection matchedAuthors = rg.Matches(value);
            string[] result = new string[matchedAuthors.Count];
            for (int i = 0; i < matchedAuthors.Count; i++)
            {
                result[i] = matchedAuthors[i].Value;
            }
            return result;
        }
        
        public static bool Regex(this string value, string pattern, out Dictionary<string,string> result)
        {
            Regex rg = new Regex(pattern); 
            Match matched = rg.Match(value);
            if (!matched.Success)
            {
                result = null;
                return false;
            }
            
            result = new(matched.Length);
            GroupCollection groupCollection = matched.Groups;
            foreach (Group group in groupCollection)
            {
                result.Add(group.Name, group.Value);
            }
            return true;
        }
    }
}