using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace az
{
    public class CommandIndexCollection
    {
        private const string _indexFileExtension = ".idx";
        private IEnumerable<CommandIndex> _indexes;

        public CommandIndexCollection(string rootPath)
        {
            _indexes = Directory.EnumerateFiles(rootPath, "*.idx", SearchOption.AllDirectories)
                .Select(f => new CommandIndex(f));
        }

        public FileInfo GetBestMatch(CommandDiscriminator discriminators)
        {
            return _indexes
                .SelectMany(i => i.Entries)
                .Where(e => e.Key.Equals(discriminators))
                .Select(e => e.Value)
                .FirstOrDefault();
        }

    }
}
