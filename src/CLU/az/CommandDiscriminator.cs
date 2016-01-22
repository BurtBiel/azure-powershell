using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az
{
    public class CommandDiscriminator
    {
        private const char seperatorChar = ' ';
        private string[] args;

        public CommandDiscriminator(string[] args) : this(string.Join(seperatorChar.ToString(), args))
        {
            
        }

        public CommandDiscriminator(string args)
        {
            Discriminator = args.ToLower();
        }

        public string Discriminator { get; private set; }

        public string[] Args
        {
            get
            {
                return Discriminator.Split(seperatorChar);
            }
        }

        public override string ToString()
        {
            return Discriminator;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.Discriminator.Equals(((CommandDiscriminator)obj).Discriminator);
        }

        public override int GetHashCode()
        {
            return Discriminator.GetHashCode();
        }
    }
}
