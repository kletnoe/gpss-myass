using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;

namespace MyAss.Framework_v2.BuiltIn.SNA
{
    public static class BlockSNA
    {
        public static double N(Simulation simulation, int blockId)
        {
            if (!simulation.Blocks.ContainsKey(blockId))
            {
                return 0;
            }

            AnyBlock block = simulation.GetBlock(blockId);

            return block.EntryCount;
        }

        public static double W(Simulation simulation, int blockId)
        {
            if (!simulation.Blocks.ContainsKey(blockId))
            {
                return 0;
            }

            AnyBlock block = simulation.GetBlock(blockId);

            return block.CurrentCount;
        }
    }
}
