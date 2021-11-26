using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_27_11
{
    class HashTable
    {
        private readonly List<BankAccount> tableAcc = new List<BankAccount>();
        private readonly List<Building> tableBuild = new List<Building>();
        internal void Add(BankAccount creation)
        {
            tableAcc.Add(creation);
        }
        internal void DeleteAcc(int index)
        {
            tableAcc.RemoveAt(index);
        }

        internal void Add(Building creation)
        {
            tableBuild.Add(creation);
        }
        internal void DeleteBuild(int index)
        {
            tableBuild.RemoveAt(index);
        }
    }
}
