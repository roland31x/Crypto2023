using MyCryptography;

namespace MonoSubtitutionCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MonoAlphabeticSubstitutionCipher mono = new MonoAlphabeticSubstitutionCipher();
            string text = "The Combinations Calculator";
            string encrypted = mono.Encrypt(text);
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine(encrypted);
            Console.WriteLine();
            Console.WriteLine(mono.Decrypt(encrypted));
            Console.WriteLine();
            Console.WriteLine(mono.Analyze(encrypted));
        }
    }
}