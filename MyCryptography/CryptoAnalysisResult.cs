using System.Buffers.Text;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MyCryptography
{
    public class CryptoAnalysisResult
    {
        public string Algorithm { get; private set; }
        List<string> Output;
        public CryptoAnalysisResult(Type shifttype, List<string> Output)
        {
            Algorithm = shifttype.Name;
            this.Output = Output;
        }
        public override string ToString()
        {
            StringBuilder message = new StringBuilder();
            message.Append("~~~" + Algorithm + " analysis~~~");
            message.Append(Environment.NewLine);
            foreach(string s in  Output)
            {
                message.Append(s);
                message.Append(Environment.NewLine);
            }
            message.Append("~~~End of analysis data~~~");
            return message.ToString();
        }
    }
}