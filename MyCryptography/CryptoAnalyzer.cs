namespace MyCryptography
{
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
}