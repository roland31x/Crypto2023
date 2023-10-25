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
            //PolyAlphabeticSubstitution ps = new PolyAlphabeticSubstitution(6);
            //string plain = "The quick brown fox jumps over the lazy dog";
            //string encrypted = ps.Encrypt(plain);
            //Console.WriteLine(plain);
            //Console.WriteLine(encrypted);
            //string decrypted = ps.Decrypt(encrypted);
            //Console.WriteLine(decrypted);
            //Playfair pf = new Playfair("playfairexample");
            //string en = pf.Encrypt("hide the gold in the tree stump");
            //Console.WriteLine(en);
            //string de = pf.Decrypt(en);
            //Console.WriteLine(de);
            //Console.WriteLine(pf.GetKey());
            JeffersonDisk jd = new JeffersonDisk(10);
            string en = jd.Encrypt("HELLOWORLD");
            Console.WriteLine(en);
            string de = jd.Decrypt(en);
            Console.WriteLine(de);
        }
    }
}