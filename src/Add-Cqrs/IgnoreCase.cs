using System;
using System.Collections.Generic;
using System.Text;

namespace Hanoog
{
    internal class IgnoreCase : EqualityComparer<string>
    {
        public override bool Equals(string x, string y)
        {
            return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode(string obj)
        {
            return base.GetHashCode();
        }
    }
}
