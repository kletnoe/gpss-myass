using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Distributions
{
    public interface IDistribution
    {
        double GetNext();
    }
}
