using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.BuiltIn.Blocks;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2.BuiltIn.Entities
{
    public class StorageEntity : AbstractEntity, IDelayableEntity
    {
        private Simulation simulation;

        public LinkedList<Transaction> DelayChain { get; private set; }

        public int Capacity { get; private set; }
        public int CurrentCount { get; private set; }
        public int EntriesCount { get; private set; }
        public int MaxContent { get; private set; }
        public int MinContent { get; private set; }
        public int RemainingCapacity { get { return this.Capacity - this.CurrentCount; } }
        public double AverageContent { get; private set; }
        public double Utilization { get; private set; }
        public double AverageHoldingTime { get; private set; }
        public bool IsEmpty { get; private set; }
        public bool IsAvaliable { get; private set; }

        public double LatestChangeClock { get; private set; }
        public double ContentTimeArea { get; private set; }

        public StorageEntity()
        {
            CurrentCount = 0;
            IsAvaliable = true;
            EntriesCount = 0;
            MaxContent = 0;
            MinContent = 0;
            AverageContent = 0.0;
            Utilization = 0.0;
            AverageHoldingTime = 0.0;

            LatestChangeClock = 0.0;
            ContentTimeArea = 0.0;
        }

        public StorageEntity(Simulation simulation, int id, int capacity)
            : this()
        {
            this.simulation = simulation;

            this.Id = id;
            this.Capacity = capacity;

            this.DelayChain = new LinkedList<Transaction>();
        }

        public void Enter(int units)
        {
            this.EntriesCount += units;
            this.UpdateStats();
            this.CurrentCount += units;

            if (this.MaxContent < this.CurrentCount)
            {
                this.MaxContent = this.CurrentCount;
            }
        }

        public void Leave(int units)
        {
            this.UpdateStats();

            this.CurrentCount -= units;

            if (this.DelayChain.Count > 0)
            {
                Transaction transaction = this.DelayChain.First();
                this.DelayChain.RemoveFirst();
                transaction.NextEventTime = this.simulation.Clock;
                ((Enter)this.simulation.Blocks[transaction.NextOwner]).PreEnter(this.simulation, transaction); // anti "line-bucking"
            }
        }

        public void SAvail()
        {
            this.IsAvaliable = true;
        }

        public void SUnavail()
        {
            this.IsAvaliable = false;
        }

        public override void UpdateStats()
        {
            this.ContentTimeArea += this.CurrentCount * (this.simulation.Clock - this.LatestChangeClock);
            this.AverageContent = this.ContentTimeArea / this.simulation.Clock;
            this.Utilization = this.ContentTimeArea / (this.Capacity * this.simulation.Clock);
            this.AverageHoldingTime = this.ContentTimeArea / this.EntriesCount;

            this.LatestChangeClock = this.simulation.Clock;
        }

        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-14} {1,4} {2,4} {3,4} {4,4} {5,6} {6,4} {7,9} {8,9} {9,5} {10,5}",
                    "STORAGE", "CAP.", "REM.", "MIN.", "MAX.", "ENTRY", "AVL.", "AVE.C", "UTIL", "RETRY", "DELAY");
        }

        public override string GetStandardReportLine()
        {
            return String.Format("{0,-14} {1,4} {2,4} {3,4} {4,4} {5,6} {6,4} {7,9:F3} {8,9:F3} {9,5} {10,5}",
                        this.simulation.NamesDictionary.GetByFirst(this.Id),
                        this.Capacity,
                        this.Capacity - this.CurrentCount,
                        this.MinContent,
                        this.MaxContent,
                        this.EntriesCount,
                        this.IsAvaliable,
                        this.AverageContent,
                        this.Utilization,
                        this.RetryChain.Count,
                        this.DelayChain.Count);
        }

        #region SNA

        public double R { get { return this.RemainingCapacity; } }
        public double S { get { return this.CurrentCount; } }
        public double SA { get { return this.AverageContent; } }
        public double SC { get { return this.EntriesCount; } }
        public double SE { get { return this.CurrentCount == 0 ? 1 : 0; } }
        public double SF { get { return this.CurrentCount == 0 ? 0 : 1; } }
        public double SR { get { return Math.Floor(this.Utilization * 1000); } }
        public double SM { get { return this.MaxContent; } }
        public double ST { get { return this.AverageHoldingTime; } }
        public double SV { get { return this.IsAvaliable ? 1 : 0; } }

        #endregion
    }
}
