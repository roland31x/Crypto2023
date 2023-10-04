namespace MyCryptography
{
    public abstract class Cipher
    {
        public abstract string Encrypt(byte[] plainText);
        public abstract string Decrypt(byte[] encryptedText);
        public abstract string Analyze(byte[] text);
    }
}