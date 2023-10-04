using MyCryptography;

namespace MonoSubtitutionCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MonoAlphabeticSubstitutionCipher mono = new MonoAlphabeticSubstitutionCipher();
            string text = "A long time ago, in a galaxy far, far away... It is a dark time for the Rebellion. Although the Death Star has been destroyed, Imperial troops have driven the Rebel forces from their hidden base and pursued them across the galaxy. Evading the dreaded Imperial Starfleet, a group of freedom fighters led by Luke Skywalker has established a new secret base on the remote ice world of Hoth. The evil lord Darth Vader, obsessed with finding young Skywalker, has dispatched thousands of remote probes into the far reaches of space…";
            string encrypted = mono.Encrypt(text);
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine(encrypted);
            Console.WriteLine();
            Console.WriteLine(mono.Decrypt(encrypted));
            Console.WriteLine();
           // Console.WriteLine(mono.Analyze(encrypted));
           
        }
    }
}