using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.Commands
{
    public class Storage : ICommand
    {
        public IDoubleOperand A_StorageCapacity { get; private set; }

        public string Label
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Storage(IDoubleOperand storageCapacity)
        {
            this.A_StorageCapacity = storageCapacity;
        }
    }
}
