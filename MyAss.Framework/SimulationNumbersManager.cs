using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework
{
    public sealed class SimulationNumbersManager
    {
        private int transactionNo;
        private int blockNo;
        private int systemNo;

        public int NextFreeTransactionNo
        {
            get
            {
                transactionNo++;
                return transactionNo;
            }
        }

        public int NextFreeBlockNo
        {
            get
            {
                blockNo++;
                return blockNo;
            }
        }

        public int NextFreeSystemNo
        {
            get
            {
                systemNo++;
                return systemNo;
            }
        }

        public SimulationNumbersManager()
        {
            this.transactionNo = 0;
            this.blockNo = 0;
            this.systemNo = 9999;
        }

        public void ResetTransactionCounter()
        {
            this.transactionNo = 0;
        }
    }
}
