using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace File_27_11
{
    class Program
    {
        static string path_in = "data.txt";
        static string path_out = "output.txt";
        static string path_excel1 = "1. Болезни.xlsx";
        static string path_excel2 = "2. Общее.xlsx";
        static Random random;
        public static Dictionary<int, Student> students;
        public static LimitQueue<Lottery> draws = new LimitQueue<Lottery>();

        static Program() //статический конструктор
        {
            random = new Random();
            students = new Dictionary<int, Student>();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Билетная лотерея");


            StreamReader sr = new StreamReader(path_in); //класс
            string[] str = sr.ReadToEnd().Split(Convert.ToChar("\n"));
            sr.Close();
            int k = 0;
            foreach (string line in str)
            {
                students.Add(k++, new Student(line.Trim().Split()[0], Convert.ToInt32(line.Trim().Split()[1])));// 0-имя 1- номер группы
            }

            foreach (int key1 in students.Keys)
            {
                Console.WriteLine($"{key1}. {students[key1]}");
            }

            while (Console.ReadLine() != "0")
            {
                Console.WriteLine("Введите название: ");
                string name = Console.ReadLine();
                Console.WriteLine("Количество билетов?");
                int num_tickets;
                if (!int.TryParse(Console.ReadLine(), out num_tickets))
                {
                    Console.WriteLine("Вы ввели что-то непонятное");
                    num_tickets = 0;
                }

                Console.Write("Количество студентов: ");
                int num_students;
                if (!int.TryParse(Console.ReadLine(), out num_students))
                {
                    Console.WriteLine("Вы ввели что-то непонятное");
                    num_students = 0;
                }

                List<int> numbers = new List<int>();

                Console.WriteLine("Номера студентов: ");
                for (int i = 0; i < num_students; i++)
                {
                    int number;
                    if (int.TryParse(Console.ReadLine(), out number) && !numbers.Contains(number))
                    {
                        numbers.Add(number);
                    }
                    else
                    {
                        Console.WriteLine("Введите снова: ");
                        i--;
                    }
                }
                double p = 0.5;
                foreach (Lottery draw in draws)
                {
                    foreach (int index in draw.Winners)
                    {
                        students[index].Koeff *= k;
                    }
                    p *= 0.5;
                }

                double koeff_sum = 0; // считаем сумму коэффициентов
                foreach (int index in numbers)
                {
                    koeff_sum += students[index].Koeff;
                }

                Stack<int> winNumbers = new Stack<int>();
                for (int i = 0; i < num_tickets; i++)
                {
                    Dictionary<double, int> pool = new Dictionary<double, int>(); //добавляем коэффициенты
                                                                                  //деленые на сумму и получаем единицу

                    double sum = 0;
                    foreach (int index in numbers)
                    {
                        sum += students[index].Koeff / koeff_sum;
                        pool.Add(sum, index);
                    }

                    double winNumber = random.NextDouble();
                    foreach (double range in pool.Keys) // пробегаемся по пулу и если победитель(победное число)
                                                        // меньше чем номер координаты студента
                    {
                        if (winNumber < range)
                        {
                            winNumbers.Push(pool[range]);
                            numbers.Remove(winNumbers.Peek());
                            koeff_sum -= students[winNumbers.Peek()].Koeff;
                            break;
                        }
                    }
                }
                draws.Enqueue(new Lottery(name, num_tickets, winNumbers));
                WriteToPath(draws.Peek().ToString());

                Console.WriteLine("Информация по трём последним розыгрышам:");
                foreach (Lottery draw in draws)
                {
                    Console.WriteLine(draw);
                }

                foreach (Lottery draw in draws)
                {
                    foreach (int index in draw.Winners)
                    {
                        students[index].Koeff = 1;
                    }
                }
            }
            Console.WriteLine("Работа с файлами");

            int columnOfKey = 0;
            string key = "диагноз";
            object[,] table = ReadExcel(path_excel1, "A1:B11");
            string[] match1 = GetColumnFromExcelTable(table, 1);
            string[] match2 = GetColumnFromExcelTable(table, 2);
            string[,] result = new string[36, 4];
            table = ReadExcel(path_excel2, "A1:H36");
            string[] titles = GetRowFromExcelTable(table, 1);
            string[] id = GetColumnFromExcelTable(table, 1);
            for (int i = 1; i < titles.Length; i++)
            {
                if (titles[i].ToLower().Equals(key))
                {
                    columnOfKey = i;
                    i = titles.Length;
                }
            }
            if (columnOfKey == 0) Environment.Exit(0);

            string[] match3 = GetColumnFromExcelTable(table, columnOfKey);
            for (int i = 2; i < match3.Length; i++)
            {
                bool done = false;
                for (int j = 2; j < match1.Length; j++)
                {
                    if (match3[i].ToLower().Contains(match1[j].ToLower()))
                    {
                        result[i, 1] = id[i];
                        result[i, 2] = match3[i];
                        if (result[i, 3] != null) result[i, 3] += "\n" + match2[j];
                        else result[i, 3] = match2[j];
                        done = true;
                    }
                }
                if (!done)
                {
                    result[i, 1] = id[i];
                    result[i, 2] = match3[i];
                    result[i, 3] = "";
                }
            }
            WriteToExcel(result);
        }
        static void ReadFromPath()
        {
            StreamReader sr = new StreamReader(path_in);
            string[] sin = sr.ReadToEnd().Trim().Split(Convert.ToChar("\n"));
            sr.Close();
            int k = 0;
            foreach (string line in sin)
            {
                try
                {
                    students.Add(k++, new Student(line.Trim().Split()[0], Convert.ToInt32(line.Trim().Split()[1])));
                }
                catch
                {
                    k--;
                }
            }
        }

        static void WriteToPath(string str)
        {
            string sout = "";
            if (File.Exists(path_out))
            {
                StreamReader sr = new StreamReader(path_out);
                sout = sr.ReadToEnd();
                sr.Close();
            }
            StreamWriter sw = new StreamWriter(path_out);
            sw.Write(sout + str);
            sw.Close();
        }

        static object[,] ReadExcel(string path, string area)
        {
            Excel.Application excel = new Excel.Application();
            excel.DisplayAlerts = false;
            Excel.Workbook book = excel.Workbooks.Open($@"{Environment.CurrentDirectory}\{path}", 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Excel.Worksheet sheet = (Excel.Worksheet)book.Sheets[1];
            object[,] table = (object[,])sheet.Range[area].Value;
            excel.Quit();
            return table;
        }

        static string[] GetColumnFromExcelTable(object[,] table, int column)
        {
            string[] column_array = new string[table.GetLength(0)];
            for (int i = 1; i < table.GetLength(0); i++)
            {
                if (table[i, column] != null)
                {
                    column_array[i] = table[i, column].ToString();
                }
                else
                {
                    i = table.GetLength(0);
                }
            }

            return column_array;
        }

        static string[] GetRowFromExcelTable(object[,] table, int row)
        {
            string[] row_array = new string[table.GetLength(1)];
            for (int i = 1; i < table.GetLength(1); i++)
            {
                if (table[row, i] != null)
                {
                    row_array[i] = table[row, i].ToString();
                }
                else
                {
                    i = table.GetLength(1);
                }
            }

            return row_array;
        }

        static void WriteToExcel(string[,] table)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            excel.SheetsInNewWorkbook = 2;
            Excel.Workbook workBook = excel.Workbooks.Add(Type.Missing);
            excel.DisplayAlerts = false;
            Excel.Worksheet sheet = (Excel.Worksheet)excel.Worksheets.get_Item(1);
            sheet.Name = "Результат";
            sheet.Cells[1, 1] = "ID";
            sheet.Cells[1, 2] = "Диагноз";
            sheet.Cells[1, 3] = "Лекарства";
            for (int i = 2; i < table.GetLength(0); i++)
            {
                for (int j = 1; j < table.GetLength(1); j++)
                {
                    sheet.Cells[i, j] = table[i, j];
                }
            }
            excel.Application.ActiveWorkbook.SaveAs($@"{Environment.CurrentDirectory}\результат.xlsx", Type.Missing,
  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            excel.Quit();
        }
    }
}

    



