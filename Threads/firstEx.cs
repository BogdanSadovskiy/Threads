using System.Collections;

namespace Threads
{
    public class ThreadAndIntArr
    {
        public int thread { get; set; }
        public int[] arr { get; set; }
        public ThreadAndIntArr(int thread, int[] arr)
        {
            this.thread = thread;
            this.arr = arr;
        }
    }
    public class firstEx
    {
        private int[] fields(int thread)
        {
            int[] arr = new int[2];
            Console.WriteLine($"Input start number for thread {thread}");
            arr[0] = Int32.Parse(Console.ReadLine());
            Console.WriteLine($"Input end number for thread {thread}");
            arr[1] = Int32.Parse(Console.ReadLine());
            return arr;
        }
        private int NumberOfThreads()
        {
            Console.WriteLine("input number of threads");
            return Int32.Parse(Console.ReadLine());
        }
        public void Run()
        {
            int numberOfThreads = NumberOfThreads();
            Hashtable ht = new Hashtable();
            for (int i = 0; i < numberOfThreads; i++)
            {
                ht.Add(i, fields(i + 1));
            }

            for (int i = 0; i < numberOfThreads; i++)
            {
                ThreadAndIntArr threadAndIntArr = new ThreadAndIntArr(i + 1, (int[])ht[i]);
                ThreadPool.QueueUserWorkItem(Ex, threadAndIntArr);
            }



        }
        private void Ex(object state)
        {
            ThreadAndIntArr descr = (ThreadAndIntArr)state;
            Console.WriteLine($"\nThread {descr.thread} start");
            int a = descr.arr[0];
            while (a <= descr.arr[1])
            {
                Console.WriteLine($"{a} ({descr.thread} thread)");
                a++;
            };
            Console.WriteLine($"Thread {descr.thread} end");
        }
    }
}

