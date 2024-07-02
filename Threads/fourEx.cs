using System;
using System.Numerics;
using System.Text.Json;




namespace Threads
{
    public class MyData
    {
        public int MIN { get; set; }
        public int MAX { get; set; }
        public int AVG { get; set; }
        public List<int> list { get; set; }

        public MyData()
        {
            list = new List<int>();
        }
    }
    public class fourEx
    {
        private ManualResetEvent minCompleted = new ManualResetEvent(false);
        private ManualResetEvent maxCompleted = new ManualResetEvent(false);
        private ManualResetEvent avgCompleted = new ManualResetEvent(false);
        private ManualResetEvent saveComplete = new ManualResetEvent(false);

        MyData myData;
        int listSize;
        int threadsCount;
        public void Run()
        {
            Console.WriteLine("Main thread Open");
            myData = new MyData();
            listSize = 10000;
            threadsCount = 4;
            GenerateArr();
            GenerateThreads();
        }
        private void GenerateThreads()
        {

            ThreadPool.QueueUserWorkItem(FindMin);
            ThreadPool.QueueUserWorkItem(FindMax);
            ThreadPool.QueueUserWorkItem(FindAvg);
            ThreadPool.QueueUserWorkItem(OutputData);
            ThreadPool.QueueUserWorkItem(SaveData);
            ThreadPool.QueueUserWorkItem(LoadData);
        }
        private void GenerateArr()
        {

            for (int i = 0; i < listSize; i++)
            {
                myData.list.Add(new Random().Next(0, 99999));
            }

        }
        private void FindMin(object o)
        {
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (FindMIN) Open");
            int tmpMin = Int32.MaxValue;
            foreach (int i in myData.list)
            {
                if (tmpMin > i) tmpMin = i;
            }
            myData.MIN = tmpMin;
            minCompleted.Set();
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (FindMIN) End");
        }

        private void FindMax(object o)
        {
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (FindMAX) Open");
            int tmpMax = Int32.MinValue;
            foreach (int i in myData.list)
            {
                if (tmpMax < i) tmpMax = i;
            }
            myData.MAX = tmpMax;
            maxCompleted.Set();
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (FindMAX) End");
        }
        private void FindAvg(object o)
        {
            Console.WriteLine($"---{Thread.CurrentThread.ManagedThreadId} thread (FindAVG) Open");
            BigInteger Sum = 0;
            foreach (int i in myData.list) Sum += i;
            BigInteger tmpAVG = Sum / listSize;
            myData.AVG = (int)tmpAVG;
            avgCompleted.Set();
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (FindAVG) End");

        }
        private void OutputData(object o)
        {
            WaitHandle.WaitAll(new WaitHandle[] { minCompleted, maxCompleted, avgCompleted });

            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Output) Open");
            Console.WriteLine($"MIN - {myData.MIN}" +
                              $"\nMAX - {myData.MAX}" +
                              $"\nAVG - {myData.AVG}");
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Output) End");
        }

        private void SaveData(object o)
        {
            WaitHandle.WaitAll(new WaitHandle[] { minCompleted, maxCompleted, avgCompleted });
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Saving) Open");
            string filePath = @"..\..\..\..\jsonFile.json";
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            SaveToJson(filePath);
            saveComplete.Set();
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Saving) End");

        }
        private void SaveToJson(string filePath)
        {

            string jsonString = JsonSerializer.Serialize(myData);
            File.WriteAllText(filePath, jsonString);
        }
        private void LoadData(object o)
        {
            saveComplete.WaitOne();
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Reading) Open");
            string filePath = @"..\..\..\..\jsonFile.json";
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            string jsonString = File.ReadAllText(filePath);
            MyData myData_ = JsonSerializer.Deserialize<MyData>(jsonString);
            Console.WriteLine("JSON File read"+ 
                             $"\nMIN - {myData_.MIN}" +
                             $"\nMAX - {myData_.MAX}" +
                             $"\nAVG - {myData_.AVG}");
            Console.WriteLine($"--- {Thread.CurrentThread.ManagedThreadId} thread (Reading) End");

        }
    }
}
