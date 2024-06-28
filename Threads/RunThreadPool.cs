
using System.Threading;

namespace Threads
{
    public class RunThreadPool
    {
        public void Run()
        {
            for (int i = 0; i < 10; i++)
            {
                int taskNumber = i;
                ThreadPool.QueueUserWorkItem(DoWork, taskNumber);
            }
            Console.WriteLine("Main Thread started");
            Thread.Sleep(1000);
            Console.WriteLine("main Thread end\n");
        }
        public void DoWork(object state)
        {
            int taskNumber = (int)state;
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} Started." +
                $"Task number: {taskNumber}");
            Thread.Sleep(2000);
            Console.WriteLine($"thread {Thread.CurrentThread.ManagedThreadId} End." +
               $"Task number: {taskNumber}\n");
        }
    }
}
