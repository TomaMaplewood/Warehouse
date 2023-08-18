using System.Collections;
using System.Collections.Generic;
using Warehouse.Model;

namespace Warehouse
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var palletList = new List<Pallet>();
                Console.Clear();
                MainMenu(ref palletList); 
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }      
        }

        private static void MainMenu(ref List<Pallet> palletList)
        {
            Console.ForegroundColor = ConsoleColor.Blue; 
            Console.Write("\nВвод данных\n " +
                  "\t C - Ручной ввод (Консоль);\n" +                
                  "\t Escape - Выход \n" +
                  "Выбор: ");
            Console.ForegroundColor = ConsoleColor.White;
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.C:
                    consolEntryMethod(ref palletList);
                    break;
                case ConsoleKey.J:
                    fileEntryMethod(ref palletList);
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default:
                    consolEntryMethod(ref palletList);
                    break;
            }

            nextMenu(palletList);
        }

        private static void nextMenu(List<Pallet> palletList)
        {
            bool run = true;

            while (run)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n Выберите вид вывода данных о складе\n" +
               "\tC - Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе " +
               "отсортировать паллеты по весу.\n" +
               "\tJ - 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема. \n" +
               "\t Экспорт в txt: \n" +
                    "\t\t Y - Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе " +
                    "отсортировать паллеты по весу.\n" +
                     "\t\tO - 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема. \n" +
               "\t E - Возврат в ввод. \n" +

               "Выбор: ");
                Console.ForegroundColor = ConsoleColor.White;
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.C:
                        foreach (var group in groupBy(palletList))
                        {
                            Console.WriteLine();
                            foreach (var item in group)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        break;

                    case ConsoleKey.J:
                        foreach (var item in outThreePallet(palletList))
                        {
                            Console.WriteLine(item.ToString());
                        }
                        break;

                    case ConsoleKey.E:
                        run = false;
                        MainMenu(ref palletList);
                        break;

                    case ConsoleKey.O:
                        using (TextWriter tw = new StreamWriter(File.Create($"..\\..\\..\\Export\\report (up to 3 pallet) -{DateOnly.FromDateTime(DateTime.Today)}.txt")))
                        {
                            foreach (var item in outThreePallet(palletList))
                            {
                                tw.Write(item.ToString());
                            }
                        }
                        Console.WriteLine($"\nОтчет создан.");
                        break;

                    case ConsoleKey.Y:
                        using (TextWriter tw = new StreamWriter(File.Create($"..\\..\\..\\Export\\report -{DateOnly.FromDateTime(DateTime.Today)}.txt")))
                        {
                            foreach (var group in groupBy(palletList))
                            {
                                 foreach (var item in group)
                                {
                                    tw.WriteLine(item.ToString());
                                }
                            }
                        }
                        Console.WriteLine($"\nОтчет создан.");
                        break;

                    default:
                        break;
                }
            }
        }
        private static IEnumerable<object> outThreePallet(in List<Pallet> palletList)
        {
            return palletList
             .OrderByDescending(x => x.ExpirationDate)
             .ThenBy(x => x.Volume)
             .ToList().GetRange(0, palletList.Count >= 3  ? 3:palletList.Count);
        }

        private static List<IGrouping<DateOnly, Pallet>> groupBy(List<Pallet> palletList)
        {
            return palletList
                .OrderBy(x => x.ExpirationDate)
                .ThenBy(x => x.Weight)
                .GroupBy(x => x.ExpirationDate)             
                .ToList();
        }

        private static void fileEntryMethod(ref List<Pallet> palletList)
        {
            throw new NotImplementedException();
        }

        private static void consolEntryMethod(ref List<Pallet> palletList)
        {
            Console.Write("\n\nКоличество паллетов: ");
            int count = int.Parse(Console.ReadLine().Trim());       

            for (int i = 0; i < count; i++)
            {
                AddPallet(ref palletList);
            }           
        }

        private static void AddPallet(ref List<Pallet> palletList)
        {
            Console.Write("Введите размеры паллета (длина ширина глубина): ");
            Int16[] size = Console.ReadLine().Split().Select(x => Convert.ToInt16(x)).ToArray();

            Pallet pallet = new Pallet()
            {
                Id = Guid.NewGuid(),
                Width = size[0],
                Height = size[1],
                Depth = size[2],
            };

            Console.Write("Количество коробок: ");

            int many = Int16.Parse(Console.ReadLine());
            Int16[] date;
            bool checkData;

            for (int i = 0; i < many; i++)
            {
                checkData = true;
                Console.Write($"№ {i + 1}. Введите размеры коробки (длина ширина глубина вес): ");
                size = Console.ReadLine().Split().Select(x => Convert.ToInt16(x)).ToArray();

                while (checkData)
                {
                    Console.Write($"\nВыбор даты для ввода: \n" +
                    $"Дата производства или срок годности: (0, 1):");
                    ConsoleKey key = Console.ReadKey().Key;

                    if (key == ConsoleKey.D0 | key == ConsoleKey.NumPad0)
                    {
                        checkData = false;
                        break;
                    }
                    else if (key == ConsoleKey.D1 | key == ConsoleKey.NumPad1)
                    {
                        checkData = true;
                        break;
                    }                  
                }

                Console.Write(checkData? $"\n№ {i + 1}. Введите дату производства коробки (31/12/2021): ": 
                    $"\n№ {i + 1}. Введите срок годности коробки (31/12/2021): ");
                date = Console.ReadLine().Split(new char[] { ' ', '.', '/' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt16(x)).ToArray();

                pallet.AddBox(new(size[0], size[1], size[2], size[3], new DateOnly(date[2], date[1], date[0]), checkData));
            }

            palletList.Add(pallet);
        }
    }
}