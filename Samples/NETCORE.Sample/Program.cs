using System;
using PublishSubscribe;

namespace NETCORE.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MessagingService.Instance.Subscribe<string>("Nick",
                (obj) => Console.WriteLine("Someone called me with args: " + obj));

            MessagingService.Instance.Subscribe("Nick",
                () => Console.WriteLine("Someone called me without args"));


            MessagingService.Instance.Send("Nick", "Yellow");
            MessagingService.Instance.Send("Nick");
            MessagingService.Instance.Send("Nick", 55);
            Console.WriteLine("Hit any key to terminate");
            Console.ReadKey();
        }
    }
}
