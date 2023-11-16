using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCryptography
{
    public class Playfair : Cipher
    {
        int[,] key = new int[5, 5];
        public string Key { get { return GetKey(); } }
        public Playfair(string startkey)
        {
            key = BuildKey(startkey);
        }
        public string Encrypt(string text, string key)
        {
            int[,] k = BuildKey(key);

            return Work(text, k, 1);
        }
        public string Decrypt(string text, string key)
        {          
            int[,] k = BuildKey(key);

            return Work(text, k, -1);           
        }
        string Work(string text, int[,] key, int dir)
        {
            text = text.ToUpper().Replace("J", "I").Replace(" ", "");

            StringBuilder sb = new StringBuilder();

            int p = 0;
            while(p < text.Length)
            {
                char A = text[p];
                p++;
                char B = 'Z';
                if(p < text.Length)
                    B = text[p];
                if (A == B)
                    B = 'X';
                else
                    p++;

                char RA = '.', RB = '.';

                (int ai, int aj) = Search(A);
                (int bi, int bj) = Search(B);

                if (ai == bi)
                {
                    RA = (char)key[ai, (5 + aj + dir) % 5];
                    RB = (char)key[bi, (5 + bj + dir) % 5];                   
                }
                else if (aj == bj)
                {
                    RA = (char)key[(5 + ai + dir) % 5, aj];
                    RB = (char)key[(5 + bi + dir) % 5, bj];
                }
                else
                {
                    RA = (char)key[ai, bj];
                    RB = (char)key[bi, aj];
                }

                sb.Append(RA);
                sb.Append(RB);
            }

            return sb.ToString();
        }
        (int,int) Search(char tofind)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (key[i, j] == tofind)
                        return (i, j);
            throw new Exception("invalid character in text");
        }
        public override string Decrypt(string text)
        {
            return Decrypt(text, Key);
        }
        int[,] BuildKey(string text)
        {
            int[,] k = new int[5, 5];
            text = text.ToUpper();
            text = text.Replace("J", "I");
            text = text.Replace(" ", "");

           // List<int> values = new List<int>();
            HashSet<int> values = new HashSet<int>();
            for (int i = 0; i < text.Length; i++)
            {
                //if (values.Contains(text[i]))
                    //continue;
                values.Add(text[i]);
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                if (i == 'J')
                    continue;
               // if (values.Contains(i))
                 //   continue;
                values.Add(i);
            }
            //for (int i = 0; i < 5; i++)
               // for (int j = 0; j < 5; j++)
                  //  k[i, j] = values[5 * i + j];
            int ii = 0;
            int jj = 0;
            foreach(var item in values)
            {
                k[ii, jj] = item;
                jj++;
                if(jj >= 5)
                {
                    ii++;
                    jj = 0;
                }
            }

            return k;
        }

        public override string Encrypt(string text)
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

        public override CryptoAnalysisResult Analyze(string text)
        {
            throw new NotImplementedException();
        }
    }
}
