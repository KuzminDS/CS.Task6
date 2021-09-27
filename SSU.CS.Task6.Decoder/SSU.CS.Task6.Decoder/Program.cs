using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSU.CS.Task6.Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Console.WriteLine("Введите путь к зашифрованному файлу");
            var filePath = Console.ReadLine();

            Console.WriteLine("Введите ключ");
            var key = Console.ReadLine();

            var decoder = new Decoder();
            decoder.Decode(filePath, key);
        }
    }
}
