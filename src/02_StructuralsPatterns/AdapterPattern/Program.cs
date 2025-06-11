using NGeoHash;
using System;
using System.Threading;

namespace AdapterPattern
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Adapter Pattern!");


            UserController controller = new UserController(new UserRepository(), new UserMapper());

            var userDto = controller.Get(1);


            Console.WriteLine(userDto);

            // RadioAdapterTest();



        }

        private static void HackPanasonic()
        {
            PanasonicRadioAdapter panasonic = new PanasonicRadioAdapter();

            panasonic.SendMessage(new Message { Content = "Hello, World!" }, 10);
        }

        private static void RadioAdapterTest()
        {

            var message = new Message { Title = "Test", Content = "Hello, World!" };

            RadioFactory radioFactory = new RadioFactory();

            while (true)
            {

                Console.WriteLine("Wybierz (M)otorola (H)ytera (P)anasonic");

                var producer = Console.ReadLine();

                IRadioAdapter radio = radioFactory.Create(producer);

                radio.SendMessage(message, 10);
            }

        }

    }




}
