using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptography
{
    public class JeffersonDisk
    {
        List<int[]> disks = new List<int[]>();
        List<int> current = new List<int>();
        public JeffersonDisk(int diskcount)
        {
            for(int t = 0; t < diskcount; t++)
            {
                int[] chars = new int['Z' - 'A' + 1];
                for (int i = 'A'; i <= 'Z'; i++)
                    chars[i - 'A'] = i;
                for (int i = chars.Length - 1; i >= 0; i--)
                {
                    int random = Cipher.rng.Next(0, i + 1);
                    (chars[i], chars[random]) = (chars[random], chars[i]);
                }
                disks.Add(chars);
                current.Add(0);
            }          
        }
        string Solve(string text, int dir)
        {
            text = text.ToUpper().Replace(" ", "");
            int idx = 0;
            StringBuilder sb = new StringBuilder();
            while (idx < text.Length)
            {
                for (int i = 0; i < disks.Count && idx < text.Length; i++, idx++)
                {
                    while (disks[i][current[i]] != text[idx])
                    {
                        current[i]++;
                        current[i] %= disks[i].Length;
                    }
                    sb.Append((char)disks[i][(current[i] + dir) % disks[i].Length]);
                }
            }
            return sb.ToString();
        }

        public string Encrypt(string text)
        {
            return Solve(text, 1);
        }
        public string Decrypt(string text)
        {
            return Solve(text, -1);
        }
            
    }
}
