using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyCryptography
{
    public class PolyAlphabeticSubstitution : Cipher
    {
        List<int[]> Keys = new List<int[]>();
        int _keylength;
        public PolyAlphabeticSubstitution(int keysize)
        {
            _keylength = keysize;
            for(int t = 0; t < _keylength; t++)
            {
                int[] chars = new int['z' - 'a' + 1];
                for (int i = 'a'; i <= 'z'; i++)
                    chars[i - 'a'] = i;
                for (int i = chars.Length - 1; i >= 0; i--)
                {
                    int random = rng.Next(0, i + 1);                
                    (chars[i], chars[random]) = (chars[random], chars[i]);
                }
                Keys.Add(chars);
            }
        }
        public override CryptoAnalysisResult Analyze(string text)
        {
            return new CryptoAnalysisResult(GetType(),new List<string> { "Polyalphabetic subtition is nearly impossible to crack and needs manual analysis..." });
        }

        public override string Encrypt(string Text)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] >= LETTERS_START_LOWER && Text[i] <= LETTERS_END_LOWER)
                    sb.Append((char)Keys[i % Keys.Count][Text[i] - 'a']);
                else if (Text[i] >= LETTERS_START_UPPER && Text[i] <= LETTERS_END_UPPER)
                    sb.Append((char)(Keys[i % Keys.Count][Text[i] - 'A'] - LOWER_TO_UPPER_DIFF));
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
                    sb.Append((char)(Array.IndexOf(Keys[i % Keys.Count], Text[i]) + 'a'));
                else if (Text[i] >= LETTERS_START_UPPER && Text[i] <= LETTERS_END_UPPER)
                    sb.Append((char)(Array.IndexOf(Keys[i % Keys.Count], Text[i] + LOWER_TO_UPPER_DIFF) + 'A'));
                else
                    sb.Append(Text[i]);
            }
            return sb.ToString();
        }
    }
}
