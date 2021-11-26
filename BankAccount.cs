using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_27_11
{
    class BankAccount
    {
            private int number, count;
            private string type;

        internal BankAccount(int number, int count, string type)
        {
            Number = number;
            Count = count;
            Type = type;
        }

        public string Type
            {
                get
                {
                    return type;
                }
                set
                {
                    if (value == null)
                    {
                        Console.WriteLine("Пользователь ничего не ввел!");
                    }
                    type = value;
                }
            }
            public int Number { get { return number; } set { number = value; } }
            public int Count { get { return count; } set { count = value; } }
            public void GetMoney(BankAccount Bank, int amount)
            {
                int Count = 0;
                Count -= amount;
                Bank.Count += amount;
            }
        }
    }

