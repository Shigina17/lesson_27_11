using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_27_11
{
    class BankFabric
    {
        public static HashTable hashTable = new HashTable(); // СОЗДАЕТСЯ ПРИ первой инициализации класса
        public static int Create(int number, int count, string type)
        {
            BankAccount account = new BankAccount(number, count, type);
            hashTable.Add(account);
            return number;   
        }
    }
}
