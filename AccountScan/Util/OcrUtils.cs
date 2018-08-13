using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AccountScan.Util
{
    class OcrUtils
    {
        private const string TESTDATA = "/Resources/tesseract-ocr/tessdata/";

        public static string Recognize(Mat mat, string lang = "eng")
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').Replace('\\', '/');
            var testData = baseDir + TESTDATA;

            CvInvoke.Threshold(mat, mat, 120, 255, ThresholdType.Binary);
            using (var tesseract = new Tesseract(testData, lang, OcrEngineMode.TesseractOnly))
            {
                tesseract.SetImage(mat);
                var i = tesseract.Recognize();
                var text = tesseract.GetUTF8Text();
                var charList = tesseract.GetCharacters();

                foreach (var ch in charList)
                {
                    var reg = ch.Region;

                    Debug.Print($"[{ch.Text}]");
                }
                return Regex.Replace(text, "\\s+", "");
            }
        }
    }
}
