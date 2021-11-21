/* К сожалению, смог сделать только так (нечитаемый код), поскольку совсем новичек и просто не хватает времени, чтобы все понять и облагородить вид.
 * Так же не успел исправить замечания по поводу выделения отдельного метода для подкрашивания строк.*/

using System;
using System.IO;

namespace Task_3
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            double erase_time = TimeSelect();
            string dir_path = DirSelect();


            double dir_size = 0;
            dir_size = FolderSize(dir_path, ref dir_size);
             if (dir_size != 0)
            {
                Console.WriteLine(@"Размер каталога ""{0}"" до очистки составляет {1} байт", dir_path, dir_size);
            }
            else
            {
                Console.WriteLine(@"Каталог ""{0}"" пуст.", dir_path);
            }
            double dir_size_before = dir_size;


            File_Deletion(dir_path, erase_time);
            Dir_Deletion(dir_path);


            dir_size = 0; // просто обнулил переменную
            dir_size = FolderSize(dir_path, ref dir_size);
            Console.WriteLine(@"Размер каталога ""{0}"" после очистки составляет {1} байт", dir_path, dir_size);
            double cleared_space = dir_size_before - dir_size;
            Console.WriteLine("\nОсвобождено: {0} байт", cleared_space);


        }

        static double FolderSize(string dir_path, ref double dir_size)
        {
            DirectoryInfo di = new DirectoryInfo(dir_path);
            DirectoryInfo[] Array_di = di.GetDirectories();
            FileInfo[] fi = di.GetFiles();

            try
            {

                foreach (FileInfo f in fi)
                {
                    dir_size += f.Length;
                }

                foreach (DirectoryInfo df in Array_di)
                {
                    FolderSize(df.FullName, ref dir_size);
                }
                return (dir_size);
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Возникла некоторая исключительная ситуация. Выполнение программы прервано!");
                Console.ResetColor();
                return 0;
            }
        }

        public static void File_Deletion(string dir_path, double erase_time)
        {
            bool time_criteria;

            try
            {
                DirectoryInfo di = new DirectoryInfo(dir_path);
                DirectoryInfo[] Array_di = di.GetDirectories();
                FileInfo[] fi = di.GetFiles();

                foreach (FileInfo f in fi)
                {
                    if (TimeSpan.Compare((DateTime.Now - File.GetLastAccessTime(dir_path)), TimeSpan.FromMinutes(erase_time)) >= 0)
                    {
                        time_criteria = true;
                    }
                    else
                    {
                        time_criteria = false;
                    }


                    if (time_criteria)
                    {
                        File.Delete(f.FullName);
                    }
                }

                foreach (DirectoryInfo df in Array_di)
                {
                    File_Deletion(df.FullName, erase_time);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\tВозникла некоторая исключительная ситуация!");
                Console.WriteLine($"\n\tОшибка: {e}");
            }
           
        }




        public static void Dir_Deletion(string dir_path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(dir_path);
                DirectoryInfo[] Array_di = di.GetDirectories();
                foreach (DirectoryInfo dri in Array_di)
                {
                    Directory.Delete(dri.FullName, true);
                    Dir_Deletion(dir_path);
                }
            }

            catch (DirectoryNotFoundException )
            {
               // Нужно постоянно отлавливать это исключение, поскольку удаляются дирректории
            }

            catch (Exception ex)
            {
                Console.WriteLine("\n\tВозникла некоторая исключительная ситуация!");
                Console.WriteLine($"\n\tОшибка: {ex}");
            }


        }




        static double TimeSelect()
        {
            Console.WriteLine("\t\nУкажите время в минутах: ");
            double erase_time;
            do
            {
                if (double.TryParse(Console.ReadLine(), out erase_time) && erase_time >= 1 && erase_time <= double.MaxValue)
                {
                    break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nВы ввели некорректное значение!\nПожалуйста, введите целое положительное число: ");
                Console.ResetColor();
            }
            while (true);
            return (erase_time);
        }



        static string DirSelect()
        {
            Console.WriteLine("\nУкажите путь до каталога: ");
            string dir_path = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Green;
            if (Directory.Exists(dir_path))
                Console.WriteLine($"\nКаталог существует, он был создан: {Directory.GetCreationTime(dir_path)} \n" +
                                    $"Время последней записи в данный каталог: {Directory.GetLastWriteTime(dir_path)} \n" +
                                    $"Время последнего обращения к данному каталогу: {Directory.GetLastAccessTime(dir_path)}");
            else
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nКаталога по данному адресу не существует!\nПожалуйста, укажите путь до каталога еще раз: ");
                    Console.ResetColor();
                    dir_path = Console.ReadLine();
                }
                while (!Directory.Exists(dir_path));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nКаталог существует, он был создан: {Directory.GetCreationTime(dir_path)} \n" +
                                    $"Время последней записи в данный каталог: {Directory.GetLastWriteTime(dir_path)} \n" +
                                    $"Время последнего обращения к данному каталогу: {Directory.GetLastAccessTime(dir_path)}");
            }
            Console.ResetColor();
            return (dir_path);
        }

    }
}
