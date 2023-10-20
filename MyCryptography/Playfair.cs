using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptography
{
    public class Playfair
    {
        int[,] key = new int[5, 5];
        public string Key { get { return GetKey(); } }
        public Playfair(string text)
        {
            key = BuildKey(text);
        }
        public string Encrypt(string text, string key)
        {
            StringBuilder sb = new StringBuilder();

            int[,] k = BuildKey(key);

            return sb.ToString();
        }
        public string Decrypt(string text, string key)
        {
            StringBuilder sb = new StringBuilder();

            int[,] k = BuildKey(key);

            return sb.ToString();
        }
        public string Decrypt(string text)
        {
            return Decrypt(text, Key);
        }
        int[,] BuildKey(string text)
        {
            int[,] k = new int[5, 5];
            text = text.ToUpper();
            text = text.Replace("J", "I");

            List<int> values = new List<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (values.Contains(text[i]))
                    continue;
                values.Add(text[i]);
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                if (i == 'J')
                    continue;
                if (values.Contains(i))
                    continue;
                values.Add(i);
            }
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    k[i, j] = values[5 * i + j];

            return k;
        }

        public string Encrypt(string text)
        {
            return Encrypt(text, Key);
        }
        public string GetKey()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    sb.Append((char)key[i, j]);
            return sb.ToString();
        }
    }
}
