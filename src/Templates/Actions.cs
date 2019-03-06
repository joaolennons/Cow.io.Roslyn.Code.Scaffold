using System;

namespace Templates
{
    [Flags]
    public enum Action
    {
        Create = 1,
        Update = 2,
        Delete = 4
    }
}
