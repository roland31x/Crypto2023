using System.Text;
using WeCantSpell.Hunspell;

namespace MyCryptography
{
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
}