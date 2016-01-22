using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace az
{
    public class CommandIndexCollection
    {
        private string[] args;
        private string rootPath;
        Dictionary<string, string> fileNamePaths;

        public CommandIndexCollection(string rootPath, string[] args)
        {
            this.args = args;
            this.rootPath = rootPath;
        }

        public string GetBestMatchPath(ref CommandDiscriminator discriminators)
        {
            string simplestMatch = args[0];
            string extToUse = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? ".cmd"
                : ".sh";
            fileNamePaths = Directory.EnumerateFiles(rootPath, $"{simplestMatch}*{extToUse}", SearchOption.AllDirectories)
                .ToDictionary(s => Path.GetFileNameWithoutExtension(s).ToLower());

            return FindMatch(fileNamePaths, ref discriminators);
        }

        private string FindMatch(Dictionary<string, string> fileNamePaths, ref CommandDiscriminator discriminators)
        {
            List<string> discriminatorList = new List<string>();
            discriminatorList.AddRange(args);

            while (discriminatorList.Count > 0)
            {
                string fileName = string.Join("-", discriminatorList);
                if (fileNamePaths.ContainsKey(fileName))
                {
                    discriminators = new CommandDiscriminator(discriminatorList.ToArray());
                    return fileNamePaths[fileName];
                }

                discriminatorList.RemoveAt(discriminatorList.Count - 1);
            }

            return null;
        }
    }
}
