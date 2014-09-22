using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2.BuiltIn.Entities
{
    public class QueueEntity : AbstractEntity
    {
        private Simulation simulation;

        private int currentContent = 0;

        private int entriesCount = 0;
        private int zeroEntriesCount = 0;
        private int maxContent = 0;
        private double averageContent = 0.0;
        private double averageTime = 0.0;
        private double averageTimeNonZero = 0.0;

        private double latestChangeClock = 0.0;
        private double contentTimeArea = 0.0;

        /// <summary>
        /// SNA::Q
        /// </summary>
        public int CurrentContent
        {
            get
            {
                return this.currentContent;
            }
        }

        // QC
        public int EntriesCount
        {
            get
            {
                return this.entriesCount;
            }
        }

        // QM
        public int MaxContent
        {
            get
            {
                return this.maxContent;
            }
        }

        // QA
        public double AverageContent
        {
            get
            {
                return this.averageContent;
            }
        }

        // QT
        public double AverageResidenceTime
        {
            get
            {
                return this.averageTime;
            }
        }

        // QX
        public double AverageResidenceTimeNonZero
        {
            get
            {
                return this.averageTimeNonZero;
            }
        }

        // QZ
        public int ZeroEntriesCount
        {
            get
            {
                return this.zeroEntriesCount;
            }
        }


        public QueueEntity(Simulation simulation, int id)
        {
            this.simulation = simulation;
            this.Id = id;
        }

        public void Queue(int units)
        {
            this.entriesCount += units;
            this.UpdateStats();
            this.currentContent += units;

            if (this.maxContent < this.currentContent)
            {
                this.maxContent = this.currentContent;
            }
        }

        public void Depart(int units)
        {
            this.UpdateStats();

            if (currentContent == units && this.latestChangeClock == this.simulation.Clock)
            {
                this.zeroEntriesCount += units;
            }

            this.currentContent -= units;
        }

        public override void UpdateStats()
        {
            this.contentTimeArea += this.currentContent * (this.simulation.Clock - this.latestChangeClock);
            this.averageContent = this.contentTimeArea / this.simulation.Clock;
            this.averageTime = this.contentTimeArea / this.entriesCount;
            this.averageTimeNonZero = this.contentTimeArea / (this.entriesCount - this.zeroEntriesCount);

            //double timeDelta = Simulation.It.Clock - this.previousChangeClock;
            //this.averageContent = ( (this.averageContent * this.previousChangeClock) + (this.currentContent * timeDelta) ) / Simulation.It.Clock;

            this.latestChangeClock = this.simulation.Clock;
        }

        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9} {6,9} {7,9} {8,6}",
                    "QUEUE", "MAX", "CONT.", "ENTRY", "ENTRY0", "AVE.CONT", "AVE.TIME", "AVE.-0", "RETRY");
        }

        public override string GetStandardReportLine()
        {
            return String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9:F3} {6,9:F3} {7,9:F3} {8,6}",
                        this.simulation.NamesDictionary.GetByFirst(this.Id),
                        this.MaxContent,
                        this.CurrentContent,
                        this.EntriesCount,
                        this.ZeroEntriesCount,
                        this.AverageContent,
                        this.AverageResidenceTime,
                        this.AverageResidenceTimeNonZero,
                        this.RetryChain.Count);
        }
    }
}
