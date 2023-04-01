using System;
using System.Collections.Generic;
using System.Text;

namespace StateEngine
{
    public interface IPrioritizedTransitionPath
    {
        uint Priority { get; }
    }
}
