using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using System;

namespace AccountScan.Util
{
    class OcrUtils
    {
        private const string TESTDATA = "/Resources/tesseract-ocr/tessdata/";

        public static Tesseract.Character[] Recognize(Mat mat, string lang = "eng")
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').Replace('\\', '/');
            var testData = baseDir + TESTDATA;

            CvInvoke.Threshold(mat, mat, 120, 255, ThresholdType.Binary);
            using (var tesseract = new Tesseract(testData, lang, OcrEngineMode.TesseractOnly))
            {
                tesseract.SetImage(mat);
                var i = tesseract.Recognize();

                return tesseract.GetCharacters();
            }
        }
    }
}
