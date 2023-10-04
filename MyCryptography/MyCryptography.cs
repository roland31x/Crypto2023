using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using WeCantSpell.Hunspell;

namespace MyCryptography
{
    public abstract class Cipher
    {
        protected static Random rng = new Random();
        protected const int LETTERS_START_LOWER = 'a';
        protected const int LETTERS_END_LOWER = 'z';
        protected const int LETTERS_START_UPPER = 'A';
        protected const int LETTERS_END_UPPER = 'Z';
        protected const int LOWER_TO_UPPER_DIFF = 'a' - 'A'; // a > A
        public abstract string Encrypt(string plainText);
        public abstract string Decrypt(string encryptedText);
        public abstract CryptoAnalysisResult Analyze(string text);
        public Cipher() { }
    }
    public class MonoAlphabeticSubstitutionCipher : Cipher
    {
        int[] Key;
        public MonoAlphabeticSubstitutionCipher()
        {
            int[] chars = new int['z' - 'a' + 1];
            for(int i = 'a'; i <= 'z'; i++)
                chars[i - 'a'] = i;
            for(int times = 0; times < 100; times++)
            {
                int random = rng.Next(0, chars.Length);
                int i = rng.Next(0, chars.Length);
                (chars[i], chars[random]) = (chars[random], chars[i]);
            }
            Key = chars;
        }
        public override string Decrypt(string Text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] >= LETTERS_START_LOWER && Text[i] <= LETTERS_END_LOWER)
                    sb.Append((char)(Array.IndexOf(Key, Text[i]) + 'a'));
                else if (Text[i] >= LETTERS_START_UPPER && Text[i] <= LETTERS_END_UPPER)
                    sb.Append((char)(Array.IndexOf(Key, Text[i] + LOWER_TO_UPPER_DIFF) + 'A'));
                else
                    sb.Append(Text[i]);
            }

            return sb.ToString();
        }

        public override string Encrypt(string plainText)
        {
            StringBuilder sb = new StringBuilder();
            
            for(int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] >= LETTERS_START_LOWER && plainText[i] <= LETTERS_END_LOWER)
                    sb.Append((char)Key[plainText[i] - 'a']);
                else if (plainText[i] >= LETTERS_START_UPPER && plainText[i] <= LETTERS_END_UPPER)
                    sb.Append((char)(Key[plainText[i] - 'A'] - LOWER_TO_UPPER_DIFF)); 
                else
                    sb.Append(plainText[i]);
            }

            return sb.ToString();
        }
        public override CryptoAnalysisResult Analyze(string text)
        {
            throw new NotImplementedException();
        }
        public string GetKey()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(int i in Key)
                stringBuilder.Append((char)i);
            return stringBuilder.ToString();
        }
    }
    public class ShiftCipher : Cipher
    {
        protected int _shift;
        public ShiftCipher()
        {

        }
        public ShiftCipher(int shift)
        {
            _shift = shift;
        }

        public override string Decrypt(string Text)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Text.Length; i++)
            {
                if ((Text[i] >= LETTERS_START_LOWER && Text[i] <= LETTERS_END_LOWER) || (Text[i] >= LETTERS_START_UPPER && Text[i] <= LETTERS_END_UPPER))
                    sb.Append(ShiftBy(Text[i],-_shift));
                else
                    sb.Append(Text[i]);
            }

            return sb.ToString();
        }

        public override string Encrypt(string plainText)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < plainText.Length; i++)
            {
                if ((plainText[i] >= LETTERS_START_LOWER && plainText[i] <= LETTERS_END_LOWER) || (plainText[i] >= LETTERS_START_UPPER && plainText[i] <= LETTERS_END_UPPER))
                    sb.Append(ShiftBy(plainText[i], _shift));
                else
                    sb.Append(plainText[i]);
            }

            return sb.ToString();
        }
        char ShiftBy(char c, int shift)
        {
            if(c < 'a')
                return (char)((('Z' - 'A' + 1) + (c - 'A') + shift) % ('Z' - 'A' + 1) + 'A');
            return (char)((('z' - 'a' + 1) + (c - 'a') + shift) % ('z' - 'a' + 1) + 'a');
        }
        string Decrypt(string text, int shift)
        {
            int temp = _shift;
            _shift = shift;
            string toreturn = Decrypt(text);
            _shift = temp;
            return toreturn;
        }
        public int GetCipherShift()
        {
            return _shift;
        }

        public override CryptoAnalysisResult Analyze(string text)
        {
            List<string> output = new List<string>
            {
                "A shift cipher shifts the alphabet with a +n offset.",
                "Manual decryption is the easiest method, as the human can just read all possible n offsets and see which actually displays readable text.",                        
            };

            string[] words = text.Split(" ");
            Array.Sort(words, (x1, x2) => x2.Length.CompareTo(x1.Length));
            string longest = words.First();

            bool foundsome = false;
            List<string> all = new List<string>();

            var dictionary = WordList.CreateFromFiles(@"/dicts/en_US.dic");

            for (int i = 0; i < 'z' - 'a' + 1; i++)
            {
                string decrypted = Decrypt(text, i);
                
                    if (dictionary.Check(Decrypt(longest, i)))
                    {
                        if (!foundsome)
                            output.Add("The decrypted text ( by using a dictionary library to recognize some words ) could be:" + Environment.NewLine);
                        output.Add(decrypted);
                        foundsome = true;
                    }
                all.Add(decrypted);
            }           
            output.Add(Environment.NewLine + "Here are all the combinations the shift cipher for human checking:");
            foreach(string s in all)
                output.Add(s);

            return new CryptoAnalysisResult(typeof(ShiftCipher), output);
        }
    }
    public class CeasarCipher : ShiftCipher
    {
        public CeasarCipher() : base(3)
        {

        }
        public override CryptoAnalysisResult Analyze(string text)
        {
            List<string> output = new List<string>
            {
                "Ceasar cipher is a shift cipher with a +3 offset.",
                "Any text can be easily decrypted without knowing the plaintext.",
                "The Decrypted text is:",
                Decrypt(text)
            };

            return new CryptoAnalysisResult(typeof(CeasarCipher), output);
        }
    }
    public class CryptoAnalyzer<T> where T : Cipher, new()
    {
        T cipher;
        public CryptoAnalyzer()
        {
            cipher = new T();
        }
        public CryptoAnalysisResult Analyze(string text)
        {
            return cipher.Analyze(text);
        }
    }

    public class CryptoAnalysisResult
    {
        public string Algorithm { get; private set; }
        List<string> Output;
        public CryptoAnalysisResult(Type shifttype, List<string> Output)
        {
            Algorithm = shifttype.Name;
            this.Output = Output;
        }
        public override string ToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append("~~~" + Algorithm + " analysis~~~");
            message.Append(Environment.NewLine);
            foreach(string s in  Output)
            {
                message.Append(s);
                message.Append(Environment.NewLine);
            }
            message.Append("~~~End of analysis data~~~");
            return message.ToString();
        }
    }
}