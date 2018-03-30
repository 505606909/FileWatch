 

 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Timers;

namespace FileWatch
{
    class Program
    { 
         
        static void Main(string[] args)
        {
            FileHelper fileHelper=new FileHelper();
                       fileHelper.WatcherStrat(); 
            Console.ReadKey();
        } 

        
         
    }
}
