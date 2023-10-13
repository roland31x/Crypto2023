using MyCryptography;

namespace ConsoleTesterApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ShiftCipher shift = new ShiftCipher(16);
            //string text = "I actually have no idea if this works properly but i hope it does.";
            //string encrypted = shift.Encrypt(text);
            //Console.WriteLine(text);
            //Console.WriteLine(encrypted);
            //Console.WriteLine(shift.Decrypt(encrypted));
            //Console.WriteLine();
            //Console.WriteLine(new CryptoAnalyzer<ShiftCipher>().Analyze(encrypted));
            PolyAlphabeticSubstitution ps = new PolyAlphabeticSubstitution(6);
            string plain = "The quick brown fox jumps over the lazy dog";
            string encrypted = ps.Encrypt(plain);
            Console.WriteLine(plain);
            Console.WriteLine(encrypted);
            string decrypted = ps.Decrypt(encrypted);
            Console.WriteLine(decrypted);
        }
    }
}