using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_27_11
{
        class Building 
        {
            private int index;
            private double high;
            private int floors;
            private int aparts;
            private int entrances;

            static int indexer = 0;

        public int Index { get => index; } // только для чтения

        public Building()
            {
                index = indexer++;
            }

            public Building(double high, int floors, int aparts, int entrances)
            {
                index = indexer++;
                this.high = high;
                this.floors = floors;
                this.aparts = aparts;
                this.entrances = entrances;
            }
        }

    }

