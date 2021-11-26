using System.Collections.Generic;

namespace File_27_11
{
    public class Lottery
    {
        public string Name;
        public int Tickets;
        public Stack<int> Winners;

        public Lottery(string name, int tickets, Stack<int> winner)
        {
            Name = name;
            Tickets = tickets;
            Winners = winner;
        }
        public override string ToString()
        {
            string str = "";
            foreach (int index in Winners)
            {
                str += Program.students[index] + "\n";
            }
            return $"Название: {Name}, кол-во билетов: {Tickets}\n{str}";
        }
    }
}