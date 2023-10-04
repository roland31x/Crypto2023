
using MyCryptography;

namespace CaesarsCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cipher ceasarCipher = new CeasarCipher();
            string text = "The quick brown fox jumps over the lazy dog";
            string encrypted = ceasarCipher.Encrypt(text);
            Console.WriteLine(text);
            Console.WriteLine(encrypted);
            Console.WriteLine(ceasarCipher.Decrypt(encrypted));
            Console.WriteLine();
            Console.WriteLine(ceasarCipher.Analyze(encrypted));

        }
    }
}