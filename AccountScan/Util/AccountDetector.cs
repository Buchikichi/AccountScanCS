using AccountScan.Data;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace AccountScan.Util
{
    class AccountDetector
    {
        public Rectangle DetectTarget(PictureInfo info, Action<int, string> act)
        {
            var region = new Rectangle();

            using (var img = info.Bitmap)
            using (var mat = new Image<Bgr, byte>(img).Mat)
            {
                var width = img.Width / 4;
                var maxY = img.Height - DETECT_HEIGHT;
                var y = 0;

                while (y < maxY)
                {
                    var rect = new Rectangle(DETECT_LEFT_MARGIN, y, width, DETECT_HEIGHT);
                    var hit = false;
                    var text = string.Empty;

                    region.X = 0;
                    act(y, string.Empty);
                    using (var clip = new Mat(mat, rect))
                    {
                        var charList = OcrUtils.Recognize(clip, "jpn");

                        foreach (var ch in charList)
                        {
                            if (string.IsNullOrWhiteSpace(ch.Text) || !TARGET_STRING.Contains(ch.Text))
                            {
                                continue;
                            }
                            hit = true;
                            text += ch.Text;
                            CvInvoke.Rectangle(clip, ch.Region, new Bgr(Color.Green).MCvScalar);
                            if (region.X == 0)
                            {
                                region = ch.Region;
                            }
                            else
                            {
                                region = Rectangle.Union(region, ch.Region);
                            }
                        }
                        act(y, string.Empty);
                        hit = text.Contains("\u5ea7\u767b");
                        if (hit)
                        {
                            CvInvoke.Imshow("source", clip);
                        }
                    }
                    if (hit)
                    {
                        region.Offset(DETECT_LEFT_MARGIN, y);
                        break;
                    }
                    y += DETECT_STEP;
                }
            }
            return region;
        }

        #region Member
        private const int DETECT_HEIGHT = 250;
        private const int DETECT_STEP = DETECT_HEIGHT / 2;
        private const int DETECT_LEFT_MARGIN = 250;
        private const string TARGET_STRING = "\u632f\u8fbc\u53e3\u5ea7\u767b\u9332";
        #endregion
    }
}
