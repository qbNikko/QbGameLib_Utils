using System.IO;
using System.Linq;
using UnityEngine;

namespace QbGameLib_Utils.Utils
{
    public class ResourceUtils
    {
        public static void GetResourceWithoutExtensionList(string path, string pattern, out string[] files)
        {
            Debug.Log(path + " " + Directory.Exists(path));
            if (!Directory.Exists(path))
            {
                files = new string[0];
                return;
            };
            files= Directory.GetFiles(path,pattern).Select(Path.GetFileNameWithoutExtension).ToArray();
        }
    }
}