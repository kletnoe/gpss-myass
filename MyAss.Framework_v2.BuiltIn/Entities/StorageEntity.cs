﻿using System;
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

        private int capacity;
        private int currentCount = 0;
        private bool avaliable = true;
        private int entriesCount = 0;
        private int maxContent = 0;
        private int minContent = 0;
        private double averageContent = 0.0;
        private double utilization = 0.0;
        private double averageTime = 0.0;

        private double latestChangeClock = 0.0;
        private double contentTimeArea = 0.0;

        private LinkedList<Transaction> delayChain;

        public int Capacity
        {
            get
            {
                return this.capacity;
            }
        }

        // S
        public int CurrentCount
        {
            get
            {
                return this.currentCount;
            }
        }

        // SC
        public int EntriesCount
        {
            get
            {
                return this.entriesCount;
            }
        }

        // SM
        public int MaxContent
        {
            get
            {
                return this.maxContent;
            }
        }

        public int MinContent
        {
            get
            {
                return this.minContent;
            }
        }

        // R
        public int RemainingCapacity
        {
            get
            {
                return this.capacity - this.currentCount;
            }
        }

        // SA
        public double AverageContent
        {
            get
            {
                return this.averageContent;
            }
        }

        // SR
        public double FractionalUtilization
        {
            get
            {
                return Math.Floor(this.utilization * 1000);
            }
        }

        public double Utilization
        {
            get
            {
                return this.utilization;
            }
        }

        // ST
        public double AverageHoldingTime
        {
            get
            {
                return this.averageTime;
            }
        }

        // SE
        public bool IsEmpty
        {
            get
            {
                return this.currentCount == 0;
            }
        }

        // SF
        public bool IsFull
        {
            get
            {
                return this.currentCount == this.capacity;
            }
        }

        // SV
        public bool IsAvaliable
        {
            get
            {
                return this.avaliable;
            }
        }

        public LinkedList<Transaction> DelayChain
        {
            get
            {
                return this.delayChain;
            }
        }

        public StorageEntity(Simulation simulation, int id, int capacity)
        {
            this.simulation = simulation;

            this.Id = id;
            this.capacity = capacity;

            this.delayChain = new LinkedList<Transaction>();
        }

        public void Enter(int units)
        {
            this.entriesCount += units;
            this.UpdateStats();
            this.currentCount += units;

            if (this.maxContent < this.currentCount)
            {
                this.maxContent = this.currentCount;
            }
        }

        public void Leave(int units)
        {
            this.UpdateStats();

            this.currentCount -= units;

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
            this.avaliable = true;
        }

        public void SUnavail()
        {
            this.avaliable = false;
        }

        public override void UpdateStats()
        {
            this.contentTimeArea += this.currentCount * (this.simulation.Clock - this.latestChangeClock);
            this.averageContent = this.contentTimeArea / this.simulation.Clock;
            this.utilization = this.contentTimeArea / (this.capacity * this.simulation.Clock);
            this.averageTime = this.contentTimeArea / this.entriesCount;

            this.latestChangeClock = this.simulation.Clock;
        }
    }
}