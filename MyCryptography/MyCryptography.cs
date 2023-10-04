using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using WeCantSpell.Hunspell;
using static System.Net.Mime.MediaTypeNames;

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
        float[] fq = new float[]
        {
            0.08167f,
            0.01492f,
            0.02782f,
            0.04253f,
            0.12702f,
            0.02228f,
            0.02015f,
            0.06094f,
            0.06966f,
            0.00153f,
            0.00772f,
            0.04025f,
            0.02406f,
            0.06749f,
            0.07507f,
            0.01929f,
            0.00095f,
            0.05987f,
            0.06327f,
            0.09056f,
            0.02758f,
            0.00978f,
            0.02360f,
            0.00150f,
            0.01974f,
            0.00074f,
        };             
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
        string Decrypt(string Text, int[] Key)
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
            List<string> output = new List<string>
            {
                "This cipher takes a random permutation of the alphabet and replaces the characters with the permutation ones.",
                "For this method we can use frequency analysis to determine the key ( permutation ) to decrypt the text.",
                "Brute forcing this would take a long time since we'd have to check 26! number of permutations.",
                "Trying to find the key by working out the letters based on the English dictionary character frequency.",
            };
            float[] currentfq = new float[fq.Length];
            int total = 0;
            string lowercase = text.ToLower();
            
            for(int i = 0; i < lowercase.Length; i++)
            {
                if (lowercase[i] >= LETTERS_START_LOWER && lowercase[i] <= LETTERS_END_LOWER)
                {
                    currentfq[lowercase[i] - 'a']++;
                    total++;
                }    
            }
            for(int i = 0; i < currentfq.Length; i++)
            {
                if(currentfq[i] != 0)
                    currentfq[i] = currentfq[i] / total;
            }
            if(total < 300)
            {
                output.Add("The text provided is really short, the chance of decryption will be low...");
            }
            output.Add("");

            int[] predictionkey = new int[Key.Length];
            bool[] basefound = new bool[fq.Length];
            bool[] newfqfound = new bool[currentfq.Length];

            for(int basefq = 0; basefq < fq.Length; basefq++)
            {
                if (basefound[basefq])
                    continue;
                float bestdist = float.MaxValue;
                int bestidx = -1;
                for(int newfq = 0; newfq < currentfq.Length; newfq++)
                {
                    if (newfqfound[newfq])
                        continue;                   
                    float dist = fq[basefq] - currentfq[newfq];
                    dist = Math.Abs(dist);
                    if (dist < bestdist)
                    {
                        bestidx = newfq;
                        bestdist = dist;
                    }
                        
                }
                predictionkey[basefq] = bestidx + 'a';
                basefound[basefq] = true;
                newfqfound[bestidx] = true;
            }

            output.Add(Decrypt(text, predictionkey));

            return new CryptoAnalysisResult(typeof(MonoAlphabeticSubstitutionCipher), output);
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

            var dictionary = WordList.CreateFromFiles(Directory.GetCurrentDirectory() + @"/dicts/en_US/en_US.dic");

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