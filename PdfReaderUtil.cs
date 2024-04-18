using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace ProfileSummaryDemo
{
    public static class PdfReaderUtil
    {
        //This method will extract the text from a PDF and return it as a string
        public static string Get(string path)
        {
            var output = "";

            using (var pdf = PdfDocument.Open(path))
            {
                foreach (var page in pdf.GetPages())
                {
                    var text = ContentOrderTextExtractor.GetText(page);
                    //This corrects for incompatible characters being read from certain PDFs
                    output += text.Replace("\0","");
                }

            }

            return output;
        }
    }
}
