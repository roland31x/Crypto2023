using MyCryptography;

namespace NShiftCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShiftCipher shift = new ShiftCipher(16);
            string text = "I actually have no idea if this works properly but i hope it does.";
            string encrypted = shift.Encrypt(text);
            Console.WriteLine(text);
            Console.WriteLine(encrypted);
            Console.WriteLine(shift.Decrypt(encrypted));
            Console.WriteLine();
            Console.WriteLine(new CryptoAnalyzer<ShiftCipher>().Analyze(encrypted));
        }
    }
}