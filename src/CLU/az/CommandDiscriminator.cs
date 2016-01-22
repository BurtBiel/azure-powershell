using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az
{
    public class CommandDiscriminator
    {
        private string[] args;

        public CommandDiscriminator(string[] args) : this(string.Join(" ", args))
        {
            
        }

        public CommandDiscriminator(string args)
        {
            // TODO, get disc part of full args
            Discriminator = args.ToLower();
        }

        public string Discriminator { get; private set; }

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
