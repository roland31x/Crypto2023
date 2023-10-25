namespace MyCryptography
{
    public abstract class Cipher
    {
        public static Random rng = new Random();
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
}