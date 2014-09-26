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

        public int CurrentContent { get; private set; }
        public int EntriesCount { get; private set; }
        public int MaxContent { get; private set; }
        public int ZeroEntriesCount { get; private set; }
        public double AverageContent { get; private set; }
        public double AverageResidenceTime { get; private set; }
        public double AverageResidenceTimeNonZero { get; private set; }

        public double LatestChangeClock { get; private set; }
        public double ContentTimeArea { get; private set; }

        public QueueEntity()
        {
            CurrentContent = 0;
            EntriesCount = 0;
            ZeroEntriesCount = 0;
            MaxContent = 0;
            AverageContent = 0.0;
            AverageResidenceTime = 0.0;
            AverageResidenceTimeNonZero = 0.0;
            LatestChangeClock = 0.0;
            ContentTimeArea = 0.0;
        }

        public QueueEntity(Simulation simulation, int id)
            : this()
        {
            this.simulation = simulation;
            this.Id = id;
        }

        public void Queue(int units)
        {
            this.EntriesCount += units;
            this.UpdateStats();
            this.CurrentContent += units;

            if (this.MaxContent < this.CurrentContent)
            {
                this.MaxContent = this.CurrentContent;
            }
        }

        public void Depart(int units)
        {
            if (this.CurrentContent < units)
            {
                throw new ModelingException("QueueEntity: Content of the Queue Entity is about to become negative!");
            }

            this.UpdateStats();

            // TODO: It isn't clear how to count ZeroEntries 'cos Queue may have subscriber witch take pairs or smth.
            if (CurrentContent == units && this.LatestChangeClock == this.simulation.Clock)
            {
                this.ZeroEntriesCount += units;
            }

            this.CurrentContent -= units;
        }

        public override void UpdateStats()
        {
            this.ContentTimeArea += this.CurrentContent * (this.simulation.Clock - this.LatestChangeClock);
            this.AverageContent = this.ContentTimeArea / this.simulation.Clock;
            this.AverageResidenceTime = this.ContentTimeArea / this.EntriesCount;
            this.AverageResidenceTimeNonZero = this.ContentTimeArea / (this.EntriesCount - this.ZeroEntriesCount);

            //double timeDelta = Simulation.It.Clock - this.previousChangeClock;
            //this.averageContent = ( (this.averageContent * this.previousChangeClock) + (this.currentContent * timeDelta) ) / Simulation.It.Clock;

            this.LatestChangeClock = this.simulation.Clock;
        }

        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9} {6,9} {7,9} {8,6}",
                    "QUEUE", "MAX", "CONT.", "ENTRY", "ENTRY0", "AVE.CONT", "AVE.TIME", "AVE.-0", "RETRY");
        }

        public override string GetStandardReportLine()
        {
            return String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9:F3} {6,9:F3} {7,9:F3} {8,6}",
                        this.simulation.NamesAndVarsDictionary.GetNameByValue(this.Id),
                        this.MaxContent,
                        this.CurrentContent,
                        this.EntriesCount,
                        this.ZeroEntriesCount,
                        this.AverageContent,
                        this.AverageResidenceTime,
                        this.AverageResidenceTimeNonZero,
                        this.RetryChain.Count);
        }

        #region SNA

        /// <summary>
        /// SNA::Q
        /// Current Queue content.
        /// </summary>
        /// <returns>Current Queue content.</returns>
        public double Q { get { return this.CurrentContent; } }

        /// <summary>
        /// SNA::QA
        /// Average Queue content.
        /// </summary>
        /// <returns>Average Queue content.</returns>
        public double QA { get { return this.AverageContent; } }

        /// <summary>
        /// SNA::QC
        /// Total queue entries.
        /// </summary>
        /// <returns>Total queue entries.</returns>
        public double QC { get { return this.EntriesCount; } }

        /// <summary>
        /// SNA::QM
        /// Maximum Queue contents.
        /// </summary>
        /// <returns>Maximum Queue contents.</returns>
        public double QM { get { return this.MaxContent; } }

        /// <summary>
        /// SNA::QT
        /// Average Queue residence time.
        /// </summary>
        /// <returns>Average Queue residence time.</returns>
        public double QT { get { return this.AverageResidenceTime; } }

        /// <summary>
        /// SNA::QX
        /// Average Queue residence time excluding zero entries.
        /// </summary>
        /// <returns>Average Queue residence time excluding zero entries.</returns>
        public double QX { get { return this.AverageResidenceTimeNonZero; } }

        /// <summary>
        /// SNA::QZ
        /// Queue zero entry count.
        /// </summary>
        /// <returns>Queue zero entry count.</returns>
        public double QZ { get { return this.ZeroEntriesCount; } }

        #endregion
    }
}
