using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.Commands
{
    public interface ICommand : IVerb
    {
        int Id { get; }
        string Label { get; }

        void Execute(Simulation simulation);
        void SetId(int id);
    }
}
