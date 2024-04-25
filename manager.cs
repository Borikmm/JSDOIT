using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;

namespace JUSTDOIT
{
    internal class manager
    {
        public static MainMenu menu;

        public static ListView TaskList;

        public static string statData = "0:0:0";

        public static Frame MainFrame { get; set; }


        private static string path = "C:\\All2\\programming\\Languages\\C#\\JUSTDOIT\\save\\save.bin";
        private static string path2 = "C:\\All2\\programming\\Languages\\C#\\JUSTDOIT\\save\\save2.txt";
        //private static string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save\\save.bin");



        public static void save_data()
        {
            File.Delete(path);
            File.WriteAllText(path, String.Empty);

            List<Task2> TaskList2 = new List<Task2>();

            foreach (var item in TaskList.Items)
            {
                var listViewItem = (Task)item;
                Task2 newTask = new Task2
                {
                    Id = listViewItem.Id,
                    Name = listViewItem.Name,
                    Discription = listViewItem.Discription,
                    Time = listViewItem.Time
                };
                TaskList2.Add(newTask);
            }

            if (File.Exists(path))
            {
                //serialize
                using (Stream stream = File.Open(path, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, TaskList2);
                }
            }

            using (FileStream fstream = new FileStream(path2, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes(statData);
                // запись массива байтов в файл
                fstream.Write(buffer, 0, buffer.Length);
            }


        }




        public static ListView load_data()
        {
            List<Task2> TaskList = new List<Task2>();



            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    TaskList = (List<Task2>)bformatter.Deserialize(stream);
                }
            }
            catch
            {
                return null;
            }


            ListView listView2 = new ListView();


            foreach (var item in TaskList)
            {
                var listViewItem = (Task2)item;
                Task newTask = new Task
                {
                    Id = listViewItem.Id,
                    Name = listViewItem.Name,
                    Discription = listViewItem.Discription,
                    Time = listViewItem.Time,
                    canvas = new Canvas(),
                    crest = new Polygon()
                };

                listView2.Items.Add(newTask);
            }



            return listView2;
        }


        public static string load_stat()
        {
            using (FileStream fstream = File.OpenRead(path2))
            {
                // выделяем массив для считывания данных из файла
                byte[] buffer = new byte[fstream.Length];
                // считываем данные
                fstream.Read(buffer, 0, buffer.Length);
                // декодируем байты в строку
                string textFromFile = Encoding.Default.GetString(buffer);
                return textFromFile;
            }
        }


    }
}
