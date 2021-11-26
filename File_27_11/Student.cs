using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_27_11
{
    class Student
    {
        public string Name;
        public int Group;
        public double Koeff;

        public Student(string name, int group)
        {
            Name = name;
            Group = group;
            Koeff = 1;
        }

        public override string ToString()
        {
            return $"Имя: {Name} - Группа: {Group}";
        }
    }
}
