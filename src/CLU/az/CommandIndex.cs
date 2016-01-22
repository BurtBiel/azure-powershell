using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace az
{
    internal class CommandIndex
    {
        private readonly char _indexFileSeparator = ':';
        private Dictionary<CommandDiscriminator, FileInfo> _indexes = new Dictionary<CommandDiscriminator, FileInfo>();

        public CommandIndex(string filePath)
        {
            foreach(var index in File.ReadAllLines(filePath))
            {
                string[] args = index.Split(_indexFileSeparator);
                _indexes.Add(new CommandDiscriminator(args[0].Replace(';', ' ')), new FileInfo(args[1]));
            }
        }

        public Dictionary<CommandDiscriminator, FileInfo> Entries
        {
            get
            {
                return _indexes;
            }
        }
    }
}