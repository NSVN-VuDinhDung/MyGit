using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Console.ReadLine();
            while (s.ToLower().Equals("exit"))
            {
                try
                {
                    new NCSw.MessgeQueue.Producer().SendMessage("Topic1", s);
                }
                catch (Exception)
                {
                    Console.WriteLine("Có lỗi!");
                }
                
                s = Console.ReadLine();
            }
        }
    }
}
