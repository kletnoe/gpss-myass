using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.Entities
{
    public interface IEntity
    {
        int Id { get; }
        LinkedList<Transaction> RetryChain { get; }

        void UpdateStats();

        string GetStandardReportHeader();
        string GetStandardReportLine();
    }
}
