using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public class QueueEntity : AbstractEntity
    {
        private int currentContent;
        private int entriesCount;
        private int maxContent;

        // Q
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
                throw new NotImplementedException();
            }
        }

        // QT
        public double AverageResidenceTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // QX
        public double AverageResidenceTimeNonZero
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // QZ
        public int ZeroEntriesCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public QueueEntity(int id)
        {
            this.Id = id;
            this.currentContent = 0;
            this.entriesCount = 0;
            this.maxContent = 0;
        }

        public void Queue(int units)
        {
            this.currentContent += units;
            this.entriesCount++;

            if (this.maxContent < this.currentContent)
            {
                this.maxContent = this.currentContent;
            }
        }

        public void Depart(int units)
        {
            this.currentContent -= units;
        }
    }
}
