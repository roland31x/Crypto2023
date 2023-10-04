using System.Text;

namespace MyCryptography
{
    public class MonoAlphabeticSubstitutionCipher : Cipher
    {
        int[] Key;
        double[] fq = new double[]
        {
            0.08167d,
            0.01492d,
            0.02782d,
            0.04253d,
            0.12702d,
            0.02228d,
            0.02015d,
            0.06094d,
            0.06966d,
            0.00153d,
            0.00772d,
            0.04025d,
            0.02406d,
            0.06749d,
            0.07507d,
            0.01929d,
            0.00095d,
            0.05987d,
            0.06327d,
            0.09056d,
            0.02758d,
            0.00978d,
            0.02360d,
            0.00150d,
            0.01974d,
            0.00074d,
        };
        const double MAXFQERROR = 0.00001d;
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

            List<int[]> predictionkeys = Predict(currentfq);

            foreach (int[] pkey in predictionkeys)
            {               
                output.Add(Decrypt(text, pkey));
            }
                

            return new CryptoAnalysisResult(typeof(MonoAlphabeticSubstitutionCipher), output);
        }
        List<int[]> Predict(float[] currentfq)
        {
            List<int[]> output = new List<int[]>();
            List<int> prediction = new List<int>();
            List<int> basefqleft = new List<int>();
            List<int> currentfqleft = new List<int>();
            for (int i = 0; i < fq.Length; i++)
            {
                basefqleft.Add(i);
                currentfqleft.Add(i);
            }
            RecursivePredict(ref output, prediction, 0, currentfqleft, currentfq);
            return output;
            
        }
        void RecursivePredict(ref List<int[]> output, List<int> currentpred,int idx , List<int> currentfqleft, float[] currentfq)
        {
            if (idx >= 'z' - 'a' + 1)
            {
                int[] key = new int[currentpred.Count];
                for(int i = 0; i < currentpred.Count; i++)
                {
                    key[i] = currentpred[i] + 'a';
                }
                output.Add(key);
                return;
            }

            List<int> nextgen = new List<int>();
            double bestdist = double.MaxValue;
            for(int j = 0; j < currentfqleft.Count; j++)
            {
                double dist = fq[idx] - currentfq[currentfqleft[j]];
                dist = Math.Abs(dist);
                if (dist < bestdist)
                {
                    bestdist = dist;
                }             
            }

            for (int j = 0; j < currentfqleft.Count; j++)
            {
                double dist = fq[idx] - currentfq[currentfqleft[j]];
                dist = Math.Abs(dist);
                if (dist < bestdist + MAXFQERROR)
                {
                    nextgen.Add(currentfqleft[j]);
                }
            }

            foreach(int next in nextgen)
            {
                List<int> newpred = new List<int>();
                foreach(int left in currentpred)
                    newpred.Add(left);
                List<int> nextcurrentleft = new List<int>();
                foreach(int left in currentfqleft)
                    nextcurrentleft.Add(left);
                nextcurrentleft.Remove(next);
                newpred.Add(next);
                RecursivePredict(ref output, newpred, idx + 1, nextcurrentleft, currentfq);
            }

        }
        public string GetKey()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(int i in Key)
                stringBuilder.Append((char)i);
            return stringBuilder.ToString();
        }
    }
}