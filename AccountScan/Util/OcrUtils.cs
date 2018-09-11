using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AccountScan.Util
{
    class OcrUtils
    {
        private const string TESTDATA = "/Resources/tesseract-ocr/tessdata/";

        public static Tesseract.Character[] Recognize(Mat mat, string lang = "eng")
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').Replace('\\', '/');
            var testData = GetShortPathName(baseDir + TESTDATA);

            CvInvoke.Threshold(mat, mat, 120, 255, ThresholdType.Binary);
            using (var tesseract = new Tesseract(testData, lang, OcrEngineMode.TesseractOnly))
            {
                tesseract.SetImage(mat);
                var i = tesseract.Recognize();

                return tesseract.GetCharacters();
            }
        }

        #region kernel32
        private static string GetShortPathName(string longPath)
        {
            var bufferSize = 260;
            var buff = new StringBuilder(bufferSize);

            GetShortPathName(longPath, buff, bufferSize);
            return buff.ToString();
        }

        [DllImport("kernel32.dll")]
        private static extern int GetShortPathName(string longPath, StringBuilder shortPathBuffer, int bufferSize);
        #endregion
    }
}
