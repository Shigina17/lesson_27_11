using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_27_11
{
    class BuildingFabric
    {
        public static HashTable hashTable = new HashTable();
        public static int Create(double high, int floors, int aparts, int entrances)
        {
            Building building = new Building(high, floors, aparts, entrances);
            return building.Index;
        }
    }
}
