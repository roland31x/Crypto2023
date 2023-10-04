namespace MyCryptography
{
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
}