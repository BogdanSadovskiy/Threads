﻿using Threads;
public class Program
{
  
    static void Main(string[] args)
    {
      firstEx first = new firstEx();
        first.Run();
        Console.WriteLine("End of main thread");
        Console.ReadKey();
    }
}